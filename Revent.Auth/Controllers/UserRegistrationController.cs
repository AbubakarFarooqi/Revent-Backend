using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.Common.Constants;
using Revent.Common.Extensions;
using Revent.Services.IServices;

namespace Revent.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly ILogger<UserRegistrationController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserRegistrationController(IMapper mapper, ILogger<UserRegistrationController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegistrationDto userRegistrationDto)
        {

            try
            {
                //this thing will be part of Sending email Api

                var isUserExist = await _userService.FindUserAsync(userRegistrationDto.Email);
                if (isUserExist != null)
                    return BadRequest(new ApiError { Message = $"User with {userRegistrationDto.Email} is already exist", StatusCode = Constants.BAD_REQUEST_STATUS_CODE });

                _logger.LogInformation("Starting user registration process.");

                // Check if the ModelState is valid
                if (!ModelState.IsValid)
                {
                    var validationErrors = ModelState.GetValidationErrorsAsCsv();
                    _logger.LogWarning("Validation failed for user registration: {Errors}", validationErrors);

                    return BadRequest(new ApiError
                    {
                        Message = validationErrors,
                        StatusCode = Constants.BAD_REQUEST_STATUS_CODE
                    });
                }

                var user = await _userService.AddUserAsync(userRegistrationDto);
                if (user == null)
                {
                    _logger.LogWarning("Failed to create user with email {Email}", userRegistrationDto.Email);

                    return BadRequest(new ApiError
                    {
                        Message = "Can't Create User",
                        StatusCode = Constants.BAD_REQUEST_STATUS_CODE
                    });
                }

                _logger.LogInformation("User {Email} has been successfully registered.", userRegistrationDto.Email);

                return Ok(new ApiResponse<object>
                {
                    Message = "User Has Been Registered!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering user with email {Email}.", userRegistrationDto.Email);

                return StatusCode(500, new ApiError
                {
                    Message = "An unexpected error occurred.",
                    StatusCode = Constants.INTERNAL_SERVER_ERROR
                });
            }

        }

    }
}
