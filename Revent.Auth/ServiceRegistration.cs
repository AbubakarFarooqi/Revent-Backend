using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.Services.IServices;
using Revent.Services.Services;

namespace Revent.Auth
{
    public static class ServiceRegistration
    {
        internal static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
           // services.AddScoped<IAspNetIdentityService, AspNetIdentityService>();
        }
    }
}
