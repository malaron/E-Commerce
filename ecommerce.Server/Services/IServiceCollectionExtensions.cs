using AutoMapper;
using eCommerce.Server.Services;
using SharedContracts;

namespace eCommerce.Services
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<UserAdminService<ApplicationUser>>();
            services.AddScoped<IMapper, Mapper>();
            return services;
        }
    }
}