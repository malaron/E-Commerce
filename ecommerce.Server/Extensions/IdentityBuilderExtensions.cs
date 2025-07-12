using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using SharedContracts;

namespace ecommerce.Server.Extensions
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddMartenStores<TUser, TRole>(this IdentityBuilder builder)
                                                where TUser : IdentityUser, IClaimsUser
                                                where TRole : IdentityRole
        {
            return builder
                        .AddRoleStore<MartenRoleStore<TRole>>()
                        .AddRoleManager<RoleManager<TRole>>()
                        .AddUserStore<ECommerceUserStore<TUser>>()
                        .AddUserManager<UserManager<TUser>>();
        }
    }
}
