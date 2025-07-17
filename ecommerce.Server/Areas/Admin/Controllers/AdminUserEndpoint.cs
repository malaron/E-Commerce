using AutoMapper;
using ecommerce.Server.Services;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedContracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections;
using Wolverine.Http;

namespace ecommerce.Server.Areas.Admin.Controllers
{
    //    [Authorize]
    [Tags("Admin Users")]
    public class AdminGetUsersEndpoint
    {
        public class UserQuery
        {
            public int PageSize { get; set; } = 10;
            public int PageNumber { get; set; } = 1;
        }

        // TODO: Maybe we should offload all user management to services.
        [WolverineGet("/Users")]
        [SwaggerResponseExample(200, typeof(SerializablePagedList<UserDTO>))]
        public SerializablePagedList<UserDTO> Users([FromQuery] UserQuery query, IDocumentStore documentStore, ILogger<ECommerceUserStore<ApplicationUser>> logger, IMapper mapper, CancellationToken token)
        {
            ECommerceUserStore<ApplicationUser> userStore = new ECommerceUserStore<ApplicationUser>(documentStore, logger);

            IQueryable<UserDTO> userDtos = documentStore.QuerySession().Query<ApplicationUser>()
                .Select(u => new UserDTO(Guid.Parse(u.Id), u.Email, u.FirstName, u.LastName));

            SerializablePagedList<UserDTO> serializablePagedList = SerializablePagedList<UserDTO>.Create(userDtos, query.PageNumber, query.PageSize);

            return serializablePagedList;

        }



        [WolverinePatch("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<UserDTO> UpdateUser([FromForm] UserDTO user, IDocumentStore documentStore, ILogger<ECommerceUserStore<ApplicationUser>> logger, IMapper mapper, CancellationToken token)
        { 
            ECommerceUserStore<ApplicationUser> userStore = new ECommerceUserStore<ApplicationUser>(documentStore, logger);
            ApplicationUser applicationUser = await userStore.FindByIdAsync(user.Id.ToString(), token);

            mapper.Map(user, applicationUser);

            await userStore.UpdateAsync(applicationUser, token);
            return user;
        }
    }
}