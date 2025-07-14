using Alba;
using Alba.Security;
using AutoMapper;
using eCommerce.Server.Services;
using ECommerce.Server;
using Marten;
using Marten.AspNetIdentity;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedContracts;
using SharedContracts.Exceptions;

namespace ECommerce_Test
{
    [TestFixture]
    public class UserAdminServiceTests
    {
        public IAlbaHost Host { get; private set; }

        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        private IServiceProvider _serviceProvider => Host.Services;
        private IMapper _mapper;

        private UserAdminService<ApplicationUser> _userAdminService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Host = await AlbaHost.For<Program>(x =>
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                x.ConfigureServices(services =>
                {
                        services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
                        .AddMartenStores<ApplicationUser, IdentityRole>()
                        .AddSignInManager<SignInManager<ApplicationUser>>()
                        .AddDefaultTokenProviders();

                    services.AddScoped<UserAdminService<ApplicationUser>>();

                    
                    services.AddSingleton<IDocumentStore>(provider =>
                    {
                        return provider.GetService<ITestStore>()!;
                    });

                    ConfigureMartenStore(services);
                });
            }, new AuthenticationStub());

            IServiceScope scope = Host.Services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            _userAdminService = serviceProvider.GetRequiredService<UserAdminService<ApplicationUser>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        [OneTimeTearDown]
        public Task OneTimeTearDown()
        {
            Host.CleanAllMartenDataAsync<ITestStore>().Wait();

            (_serviceProvider as IDisposable)?.Dispose();

            if (Host != null)
            {
                return Host.DisposeAsync().AsTask();
            }

            _userManager.Dispose();
            return Task.CompletedTask;
        }

        [Test, Order(1)]
        public async Task Should_Get_All_Users()
        {
            ICollection<UserDTO> result = await _userAdminService.GetUsers();
            Assert.That(result?.Count, Is.EqualTo(2));
        }

        [Test, Order(2)]
        public async Task Should_Add_User()
        {
            UserCreationRequestDTO userCreationRequestDTO = new UserCreationRequestDTO
            (
                "ApplicationUser3@gmail.com",
                "Threefer",
                "Usermang",
                "Root!23",
                "Root!23"
            );

            UserCreationResponseDTO response =await _userAdminService.RegisterUser(userCreationRequestDTO);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Token, Is.Not.Null.Or.WhiteSpace);

            var users = await _userAdminService.GetUsers();

            Assert.That(users?.Count, Is.EqualTo(3));
        }

        [Test, Order(3)]
        public void Should_Fail_To_Add_Existing_User()
        {
            UserCreationRequestDTO userCreationRequestDTO = new UserCreationRequestDTO
            (
                "ApplicationUser@gmail.com",
                "Test",
                "User",
                "Root!23",
                "Root!23"
            );

            Assert.ThrowsAsync<UserCreationException>(async () => await _userAdminService.RegisterUser(userCreationRequestDTO));
        }

        [Test, Order(4)]
        public void Should_Fail_To_Add_Invalid_Password()
        {
            UserCreationRequestDTO userCreationRequestDTO = new UserCreationRequestDTO
            (
                "ApplicationUser@gmail.com",
                "Test",
                "User",
                "root",
                "root"
            );

            Assert.ThrowsAsync<UserCreationException>(async () => await _userAdminService.RegisterUser(userCreationRequestDTO));
        }

        [Test, Order(5)]
        public async Task Should_Update_User()
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync("ApplicationUser@gmail.com");

            UserDTO userDTO = new UserDTO(new Guid(user!.Id), "ApplicationUserzz@gmail.com", "Test", "User");

            UserDTO result = await _userAdminService.UpdateUser(userDTO);

            Assert.That(result.FirstName, Is.EqualTo("Test"));
            Assert.That(result.LastName, Is.EqualTo("User"));
            Assert.That(result.Email, Is.EqualTo("ApplicationUserzz@gmail.com"));

        }

        [Test, Order(6)]
        public void Should_Fail_To_Update_User()
        {
            UserDTO userDTO = new UserDTO(Guid.NewGuid(), "ApplicationUser@gmail.com", "Test", "User");

            Assert.That(async () => await _userAdminService.UpdateUser(userDTO), Throws.Exception.TypeOf<UserNotFoundException>());
        }

        private void ConfigureMartenStore(IServiceCollection services)
        {
            MartenServiceCollectionExtensions.MartenStoreExpression<ITestStore> store = services.AddMartenStore<ITestStore>(opts =>
                opts.Connection(_configuration.GetConnectionString("PostgresTestConnectionString")!));

            ITestStore testStore = services.BuildServiceProvider().GetService<ITestStore>()!;
            IServiceScopeFactory scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>()!;
            store.InitializeWith(new BaselineData(scopeFactory, testStore));
        }
    }
    internal class BaselineData : IInitialData
    {
//        private UserManager<ApplicationUser> _userManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ITestStore _store;
        public BaselineData(IServiceScopeFactory scopeFactory, ITestStore store)
        {
            _scopeFactory = scopeFactory;
            _store = store;
        }

        public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
        {
            
            await using IDocumentSession session = (_store).LightweightSession();

            ApplicationUser appUser = new()
            {
                UserName = "ApplicationUser@gmail.com",
                Email = "ApplicationUser@gmail.com",
                EmailConfirmed = true,
                FirstName = "Test",
                LastName = "User"
            };

            ApplicationUser appUser2 = new()
            {
                UserName = "ApplicationUser2@gmail.com",
                Email = "ApplicationUser2@gmail.com",
                EmailConfirmed = true,
                FirstName = "Twofer",
                LastName = "Usermang"
            };



            using (var scope = _scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                await userManager.CreateAsync(appUser, "Root!23");
                await userManager.CreateAsync(appUser2, "Root!23");
            }
        }
    }
}
