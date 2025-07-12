using AutoMapper;
using ecommerce.Server.Services;
using eCommerce.Data;
using Marten;
using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using SharedContracts;
using SharedContracts.Enum;
using SharedContracts.Exceptions;
using static SharedContracts.Events;

namespace eCommerce.Server.Services
{
    public class UserAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDocumentStore _documentStore;
        private readonly IMapper _mapper;
        private readonly ECommerceUserStore<ApplicationUser> _userStore;
        private readonly ILogger<ECommerceUserStore<ApplicationUser>> _logger;

        public UserAdminService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IDocumentStore documentStore, IMapper mapper, ILogger<ECommerceUserStore<ApplicationUser>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _documentStore = documentStore;
            _mapper = mapper;
            _logger = logger;
            _userStore = new ECommerceUserStore<ApplicationUser>(_documentStore, _logger);

        }

        public async Task<UserCreationResponseDTO> RegisterUser(UserCreationRequestDTO user)
        {
            ApplicationUser appUser = new()
            {
                UserName = user.Email,
                Email = user.Email,
                EmailConfirmed = true,
                FirstName = user.FirstName,
                LastName = user.LastName,

            };

            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));


            if (result.Succeeded)
            {
                KeyLoader.RsaKeys keys = await KeyLoader.LoadRsaKeysFromPem();
                UserCreationResponseDTO userCreationResponse = new(
                    TokenGenerator.GenerateJwtToken(keys, user.Email, "test.com", "test.com"));
                
                return userCreationResponse;
            }

            string[] errors = result.Errors.Select(e => e.Description).ToArray();
            throw new UserCreationException(errors);
        }

        public async Task<UserLoginResultDTO> LoginUser(UserLoginRequestDTO userLogin)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.RememberMe, true);
            string token = null!;

            if (signInResult.Succeeded)
            {
                KeyLoader.RsaKeys keys = await KeyLoader.LoadRsaKeysFromPem();
                token = TokenGenerator.GenerateJwtToken(keys, userLogin.Email, "test.com", "test.com");
            }

            LoginResult result = signInResult.Succeeded ? LoginResult.Success
                 : signInResult.RequiresTwoFactor ? LoginResult.RequiresTwoFactor
                 : signInResult.IsLockedOut ? LoginResult.IsLockedOut
                 : LoginResult.InvalidAttempt;

            return new UserLoginResultDTO(result, token);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        // TODO: This shouldn't return an Application User, it should return a UserDTO
        public async Task<ICollection<UserDTO>> GetUsers()
        {
            IDocumentSession documentSession = _documentStore.LightweightSession();

            var users = await documentSession.Query<ApplicationUser>().ToListAsync();

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            UserFirstNameUpdated userFirstNameUpdated = new(user.FirstName);
            UserLastNameUpdated userLastNameUpdated = new(user.LastName);
            UserEmailUpdated userEmailUpdated = new(user.Email);

            IDocumentSession documentSession = _documentStore.LightweightSession();

            documentSession.Events.StartStream<ApplicationUser>(user.Id, userFirstNameUpdated, userLastNameUpdated, userEmailUpdated);

            await documentSession.SaveChangesAsync();

            return user;
        }
    }
}
