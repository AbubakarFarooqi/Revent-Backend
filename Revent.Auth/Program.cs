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

//Add Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config.GetConnectionString("RedisCacheSettings:ConnectionString");
    options.InstanceName = config["RedisCacheSettings:InstanceName"];
});

// Configures ASP.NET Identity Framework
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
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

// configuring Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // SignIn settings
    options.SignIn.RequireConfirmedAccount = true; // Set to true if you require email confirmation
});



// Configure Services
//builder.Host.ConfigureServices(Revent.Auth.ServiceRegistration.RegisterServices);
builder.Host.ConfigureServices((context, services) =>
    Revent.Auth.ServiceRegistration.RegisterServices(services, context.Configuration)
);
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
