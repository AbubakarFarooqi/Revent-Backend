
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniValidation;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.Common.Constants;
using Revent.Services.IServices;
using Revent.Services.Services;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Globals
var cloudinaryOptions = builder.Configuration.GetSection("CloudinaryOptions").Get<CloudinaryOptions>();



//Add logger
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Adding services in DI container
var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
var cloudinary = new CloudinaryDotNet.Cloudinary(new Account(
    cloudinarySettings.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret
));
builder.Services.AddSingleton(cloudinary);
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// APIs
app.MapPost("/uploadImage", async (HttpContext httpContext, ICloudinaryService _cloudinaryService, [FromForm] UploadImageDto imageDto) =>
{
    try
    {
        // Validate the model manually
        if (!httpContext.Request.HasFormContentType || !httpContext.Request.Form.Files.Any())
        {
            return Results.BadRequest("No file uploaded.");
        }

        if (!MiniValidator.TryValidate(imageDto, out var errors))
        {
            return Results.BadRequest(errors);
        }
        string? url;
        long? size;
        (url, size) = await _cloudinaryService.UploadImage(imageDto.Picture, cloudinaryOptions.ImageQuality);

        return Results.Ok(new ApiResponse<UploadImageResponseDto> { StatusCode = Constants.OK_STATUS_CODE, Data = new UploadImageResponseDto { Size = size,Url = url} });

    }
    catch (Exception ex)
    {
        return Results.Json(new ApiError { Message = ex.ToString(), StatusCode = Constants.INTERNAL_SERVER_ERROR }, statusCode: Constants.INTERNAL_SERVER_ERROR);
    }
}).DisableAntiforgery();


app.Run();

