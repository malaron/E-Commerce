using Marten.AspNetIdentity;
using Marten.Events.Aggregation;
using Microsoft.AspNetCore.Identity;

namespace SharedContracts
{
    public class ApplicationUser : IdentityUser, IClaimsUser
    {
        public IList<string> RoleClaims { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
