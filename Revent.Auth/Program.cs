using Autofac.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.Auth;
using Revent.Common.CommonModels;
using Revent.Auth.Controllers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


// Add services to the container.
builder.Services.AddHttpClient<AuthController>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Auto Mapper 
builder.Services.AddAutoMapper(Assembly.Load("Revent.Common"));
// Configures ASP.NET Identity Framework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"));
    options.UseOpenIddict();
});

builder.Services.AddDbContext<ReventDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"));
});

// Configure OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(coreOptions =>
    {
        coreOptions.UseEntityFrameworkCore()
                    .UseDbContext<AuthDbContext>();
    })
    .AddServer(options =>
    {
        options.RegisterScopes(
            "api",
            "offline_access"
        )
        .AllowClientCredentialsFlow().AllowRefreshTokenFlow()
        .AllowPasswordFlow().AllowRefreshTokenFlow().SetAccessTokenLifetime(TimeSpan.FromMinutes(30))
        .SetRefreshTokenLifetime(TimeSpan.FromDays(7))

        .SetTokenEndpointUris("connect/token")
        .AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate()
        .DisableAccessTokenEncryption()

        // Register the ASP.NET Core host and configure the ASP.NET Core options.
        .UseAspNetCore()
        .EnableTokenEndpointPassthrough()
        .DisableTransportSecurityRequirement();
        // For those users which don't send client id and secret
        options.AcceptAnonymousClients();
    });

// Configure Services
builder.Host.ConfigureServices(Revent.Auth.ServiceRegistration.RegisterServices);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins(config.GetValue<string>("AllowedOrigins") ?? "")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
