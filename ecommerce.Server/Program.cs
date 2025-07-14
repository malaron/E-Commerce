
using ecommerce.Server.Extensions;
using ecommerce.Server.Services;
using ecommerce.Server.Services.MappingProfiles;
using eCommerce.Data;
using eCommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedContracts;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Server
{
    public class Program
    {
        private static string _postgresConnectionString { get; set; } = null!;

        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            _postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnectionString")!;
            await ConfigureServices(builder.Services, builder.Environment);

            WebApplication app = builder.Build();

            ConfigureApp(app);

            app.Run();
        }

        private static async Task ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            // Add services to the container.
            services.AddLogging();

            KeyLoader.RsaKeys rsaKeys = await KeyLoader.LoadRsaKeysFromPem();

            RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(rsaKeys.PublicKeyBytes, out _);
            RSAParameters rsaParameters = rsa.ExportParameters(false);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "test.com",
                    ValidAudience = "test.com",
                    IssuerSigningKey = new RsaSecurityKey(rsaParameters)
                };
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddControllers();

            services.AddDataConnector(_postgresConnectionString, environment.IsDevelopment());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddMartenStores<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce", Version = "v1" });
                c.CustomSchemaIds(schemaIdStrategy);
                c.TagActionsBy(apiDesc => apiDesc.GetAreaName());
            });
            services.AddEndpointsApiExplorer();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.RegisterServices();

        }


        private static void ConfigureApp(WebApplication app)
        {
            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "my_store v1"));
            }

            app.UseHttpsRedirection();

            app.MapControllerRoute(
                name: "Area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

        }
        private static string schemaIdStrategy(Type currentClass)
        {
            string returnedValue = currentClass.Name.Replace("DTO", string.Empty);
            if (returnedValue.EndsWith("DTO"))
                returnedValue = returnedValue.Replace("DTO", string.Empty);
            return returnedValue;
        }

    }
}
