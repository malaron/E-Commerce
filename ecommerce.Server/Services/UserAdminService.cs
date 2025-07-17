using AutoMapper;
using ecommerce.Server.Services;
using Marten;
using Microsoft.AspNetCore.Identity;
using SharedContracts;
using SharedContracts.Enum;
using SharedContracts.Exceptions;

namespace eCommerce.Server.Services
{
    public class UserAdminService<T> where T : ApplicationUser, new()
    {
        private readonly UserManager<T> _userManager;
        private readonly SignInManager<T> _signInManager;
        private readonly IDocumentStore _documentStore;
        private readonly IMapper _mapper;
        private readonly ECommerceUserStore<T> _userStore;
        private readonly ILogger<ECommerceUserStore<T>> _logger;

        public SignInManager<T> SignInManager => _signInManager;

        public UserAdminService(UserManager<T> userManager, SignInManager<T> signInManager, IDocumentStore documentStore, IMapper mapper, ILogger<ECommerceUserStore<T>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _documentStore = documentStore;
            _mapper = mapper;
            _logger = logger;
            _userStore = new ECommerceUserStore<T>(_documentStore, _logger);
        }

        public async Task<UserCreationResponseDTO> RegisterUser(UserCreationRequestDTO user)
        {
            T appUser = new()
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

            
            var users = await documentSession.Query<T>().ToListAsync();

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            T? storedUser = await _userManager.FindByIdAsync(user.Id.ToString()) ?? throw new UserNotFoundException();

            _mapper.Map(user, storedUser);

            IdentityResult result = await _userManager.UpdateAsync(storedUser);

            T? updatedUser = await _userManager.FindByIdAsync(user.Id.ToString());

            UserDTO userDTO = _mapper.Map<UserDTO>(updatedUser);
            return userDTO;
        }
    }
}
