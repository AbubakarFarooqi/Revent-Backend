using Google.Apis.Auth;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.Server.AspNetCore;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static Revent.Auth.Controllers.AuthController;
using Revent.Auth.DTOs;
using Revent.Services.IServices;
using Revent.Common.CommonDtos;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using Revent.Common.CommonModels;

namespace Revent.Auth.Controllers
{
    [ApiController]

    public class AuthController : ControllerBase
    {
        IConfiguration config;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IUserService _userService;

        public AuthController(IConfiguration config, HttpClient httpClient, IHttpClientFactory httpClientFactory, IOpenIddictApplicationManager applicationManager,IUserService userService
            )
        {
            this.config = config;
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
            _applicationManager = applicationManager;
            _userService = userService;
        }

        [HttpPost]
        [Route("connect/Token")]
        public async Task<IActionResult> ConnectToken()
        {
            try
            {
                var openIdConnectRequest = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal();
                ApplicationUser? user = null;

                var email = openIdConnectRequest.Username;

                if (openIdConnectRequest.IsClientCredentialsGrantType())
                {
                    var name = openIdConnectRequest.GetParameter("name")?.ToString() ?? string.Empty;
                    var subject = openIdConnectRequest.GetParameter("subject")?.ToString() ?? string.Empty;
                    var picture = openIdConnectRequest.GetParameter("picture")?.ToString() ?? string.Empty;

                    if (string.IsNullOrEmpty(name)) throw new ArgumentException("The Name cannot be null or empty", nameof(name));
                    if (string.IsNullOrEmpty(email)) throw new ArgumentException("The Email cannot be null or empty", nameof(email));
                    if (string.IsNullOrEmpty(subject)) throw new ArgumentException("The subject cannot be null or empty", nameof(subject));

                    user = await _userService.FindUserAsync(email);
                    if (user == null) 
                    {
                        UserRegistrationDto userDto = new UserRegistrationDto
                        {
                            FullName = name,
                            Email = email,
                            ProfilePicture = picture,
                        };
                        user = await _userService.AddUserWithoutPasswordAsync(userDto);
                    }
                    
                }
                else if (openIdConnectRequest.IsPasswordGrantType())
                {
                    var password = openIdConnectRequest.Password;

                    if (string.IsNullOrEmpty(email)) throw new ArgumentException("The Email cannot be null or empty", nameof(email));
                    if (string.IsNullOrEmpty(password)) throw new ArgumentException("The password cannot be null or empty", nameof(password));

                    user = await _userService.FindUserAsync(email);
                   
                    if (user == null) return BadRequest();
                    if (await _userService.CheckPasswordAsync(user,password ?? "")) return BadRequest();
                   
                }

                if ( user == null) return BadRequest();
                
                List<string>? userRoles;

                userRoles = await _userService.GetUserRoles(user);
                var roleClaims = userRoles?.Select(role => new Claim("role", role)).ToList();

                var claims = new List<Claim>
                    {
                        new Claim(OpenIddictConstants.Claims.Subject, email),
                    };
                
                if(roleClaims != null) claims.AddRange(roleClaims);

                identity.AddClaims(claims);
                identity.SetDestinations(GetDestinations);
                principal = new ClaimsPrincipal(identity);
                principal.SetScopes(new[]
                {
                    Scopes.OpenId,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                });

                identity.SetDestinations(GetDestinations);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        [Route("api/auth/google/callback")]
        public async Task<IActionResult> GoogleAuthCallBackForWebApp(string code)
        {
            try
            {
                // Exchange the authorization code for an access token
                var googleTokenResponse = await ExchangeCodeForTokenAsync(code);

                if (googleTokenResponse == null || string.IsNullOrEmpty(googleTokenResponse.id_token))  return Redirect("http://localhost:3000/failedGoogleAuth");

                // Validate the Google ID token
                var validPayload = await ValidateGoogleToken(googleTokenResponse.id_token);

                if (validPayload == null)   return Redirect("http://localhost:3000/failedGoogleAuth");

                var httpClient = _httpClientFactory.CreateClient();

                //Making Token Request
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, config["AuthServer:TokenUrl"])
                {
                    Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", config["AuthServer:AuthServerClientId"] ?? ""),
                        new KeyValuePair<string, string>("client_secret", config["AuthServer:AuthServerClientSecret"] ?? ""),
                        new KeyValuePair<string, string>("name", validPayload.Name),
                        new KeyValuePair<string, string>("username", validPayload.Email),
                        new KeyValuePair<string, string>("subject", validPayload.Subject),
                        new KeyValuePair<string, string>("picture", validPayload.Picture)
                        })
                };

                var response = await httpClient.SendAsync(tokenRequest);

                if(!response.IsSuccessStatusCode) return Redirect("http://localhost:3000/failedGoogleAuth");

                var content = await response.Content.ReadAsStringAsync();

                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponseDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (tokenResponse == null)  return Redirect("http://localhost:3000/failedGoogleAuth");
                
                var redirectUrl = $"http://localhost:3000/dashboard?access_token={tokenResponse?.access_token}&refresh_token={tokenResponse?.refresh_token}&expires_in={tokenResponse?.expires_in}";
                return Redirect(redirectUrl);
            }
            catch(Exception ex)
            {
                return Redirect("http://localhost:3000/failedGoogleAuth");
            }
        }







       /* [HttpPost]
        public async Task<IActionResult> RegisterApplication() 
        {
            _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "AuthServerClient",
                ClientSecret = "AuthServerClient",
                DisplayName = "AuthServerClient",
                Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.Password,
                        Permissions.Prefixes.Scope + "api",
                        Permissions.Prefixes.Scope + Scopes.OfflineAccess,
                    },
            }).GetAwaiter().GetResult();
        }*/
        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
        {
            var googleSection = config.GetSection("Authentication:Google:WebApp");
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { googleSection["ClientId"] ?? "" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload;
        }
        private async Task<GoogleTokenResponseDto?> ExchangeCodeForTokenAsync(string code)
        {
            var googleSection = config.GetSection("Authentication:Google:WebApp");

            var clientId = googleSection["ClientId"];
            var clientSecret = googleSection["ClientSecret"];
            var redirectUri = "https://localhost:7068/api/auth/google/callback";

            var requestData = new StringContent(JsonConvert.SerializeObject(new
            {
                code = code,
                client_id = clientId,
                client_secret = clientSecret,
                redirect_uri = redirectUri,
                grant_type = "authorization_code"
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", requestData);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GoogleTokenResponseDto>(jsonResponse);
            }
            return null;
        }
        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            return new[] { Destinations.AccessToken, Destinations.IdentityToken };
        }
      
    }
}
