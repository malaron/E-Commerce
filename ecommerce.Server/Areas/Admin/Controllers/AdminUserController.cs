using eCommerce.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedContracts;

namespace ecommerce.Server.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Area("Admin")]
    [Consumes("multipart/form-data")]
    [Route("Admin/[controller]/[action]")]
    public class AdminUserController : ControllerBase
    {

        private readonly UserAdminService<ApplicationUser> _userAdmin;

        public AdminUserController(UserAdminService<ApplicationUser> userAdmin)
        {
            _userAdmin = userAdmin;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserDTO>>> Users()
        {
            return Ok( await _userAdmin.GetUsers());
        }

        [HttpPatch]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateUser([FromForm] UserDTO user)
        {
            return CreatedAtAction(nameof(UserDTO), await _userAdmin.UpdateUser(user));
        }
    }
}
