using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Revent.DataAccess.Implementation.DbContexts;
using System.Reflection;
using Revent.Common.CommonModels;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add Controllers
builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.PropertyNamingPolicy = null;
      }); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "provide jwt",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme,
        }

    };
    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme ,Array.Empty<string>() }
    });
});

//Add Auto Mapper 
builder.Services.AddAutoMapper(Assembly.Load("Revent.Common"));

//Add Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config["RedisCacheSettings:ConnectionString"];
    options.InstanceName = config["RedisCacheSettings:InstanceName"];
});

// Configure database contexts
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"))
);

builder.Services.AddDbContext<ReventDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultDbConnectionString"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = config.GetSection("AuthServerSettings").GetValue<string>("BaseUrl");
    options.Audience = "Resource";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.NameClaimType = "username";
    options.TokenValidationParameters.RoleClaimType = System.Security.Claims.ClaimTypes.Role;
});

// Use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configure Services
builder.Host.ConfigureServices((context, services) =>
    Revent.WebApi.ServiceRegistration.RegisterServices(services, context.Configuration)
);

// Configure CORS
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

// Logging for debugging purposes
var logger = app.Services.GetRequiredService<ILogger<Program>>();

var contentPath = Path.Combine(builder.Environment.ContentRootPath, "Content");
logger.LogInformation("Serving static files from: {ContentPath}", contentPath);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(contentPath),
    RequestPath = "/Content"
});*/

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
