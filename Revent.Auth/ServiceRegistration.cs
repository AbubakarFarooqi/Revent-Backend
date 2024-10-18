using CloudinaryDotNet;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.Services.IServices;
using Revent.Services.Services;

namespace Revent.Auth
{
    public static class ServiceRegistration
    {
        internal static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

           // Register Cloudinary service
           var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var cloudinary = new CloudinaryDotNet.Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            ));
            services.AddSingleton(cloudinary);
        }
    }
}
