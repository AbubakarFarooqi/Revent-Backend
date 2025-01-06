using CloudinaryDotNet;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.Services.IServices;
using Revent.Services.Services;

namespace Revent.WebApi
{
    public class ServiceRegistration
    {
        internal static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            // Register Cloudinary service
            var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var cloudinary = new CloudinaryDotNet.Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            ));
            services.AddSingleton(cloudinary);

            //Register RabbitMQ
            //services.Configure<RabbitMQSetting>(configuration.GetSection("RabbitMQ"));
        }
    }
}
