using SharedContracts;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Server.Services;
using SharedContracts.Enum;
using Microsoft.AspNetCore.Authorization;

namespace ecommerce.Server.Controllers
{
    [ApiController]
    [Consumes("multipart/form-data")]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserAdminService<ApplicationUser> _adminService;

        public AccountController(UserAdminService<ApplicationUser> adminService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _adminService.Logout();
            _logger.LogInformation("User logged out.");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return BadRequest(new
            {
                Message = "User logged out."
            });

        }

        [HttpPost]
        [ProducesResponseType(typeof(UserCreationResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromForm] UserCreationRequestDTO user)
        {
            return Ok(await _adminService.RegisterUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginRequestDTO userLogin)
        {
            UserLoginResultDTO result = await _adminService.LoginUser(userLogin);

            if (result.LoginResult == LoginResult.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
