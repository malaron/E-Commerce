using Marten;
using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContracts
{
    public class ECommerceUserStore<TUser> : MartenUserStore<TUser> where TUser : IdentityUser, IClaimsUser
    {
        public ECommerceUserStore(IDocumentStore documentStore, ILogger<ECommerceUserStore<TUser>> logger) : base(documentStore, logger)
        {
        }
    }
}
