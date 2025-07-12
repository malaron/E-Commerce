using AutoMapper;
using ecommerce.Server.Services;
using eCommerce.Server.Services;

namespace eCommerce.Services
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<UserAdminService>();
            services.AddScoped<IMapper, Mapper>();
            return services;
        }
    }
}