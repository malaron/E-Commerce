
using ecommerce.Server.Extensions;
using ecommerce.Server.Services;
using eCommerce.Data;
using eCommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedContracts;
using SharedContracts.MappingProfiles;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Cryptography;
using Wolverine;
using Wolverine.Http;

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

            builder.Host.UseWolverine(options =>
            {
                options.Policies.AutoApplyTransactions();
            });
             
            WebApplication app = builder.Build();

            ConfigureApp(app);

            app.Run();
        }

        private static async Task ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddLogging();

            AddProblemDetails(services);

            services.AddWolverineHttp();

            await AddAuthentication(services);

            services.AddHttpContextAccessor();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddControllers();

            services.AddDataConnector(_postgresConnectionString, environment.IsDevelopment());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddMartenStores<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();

            services.ConfigureSystemTextJsonForWolverineOrMinimalApi(o =>
            {
                // Do whatever you want here to customize the JSON
                // serialization
                o.SerializerOptions.WriteIndented = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce", Version = "v1" });
                c.CustomSchemaIds(schemaIdStrategy);
                c.SchemaFilter<GenericFilter>();
            });
            services.AddEndpointsApiExplorer();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.RegisterServices();

        }

        private static async Task AddAuthentication(IServiceCollection services)
        {
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
        }

        private static void AddProblemDetails(IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            });
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
            app.MapWolverineEndpoints(opts =>
            {
                opts.UseNewtonsoftJsonForSerialization();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapFallbackToFile("/index.html");

        }
        private static string schemaIdStrategy(Type currentClass)
        {
            string returnedValue = currentClass.Name.Replace("DTO", string.Empty);
            if (returnedValue.EndsWith("DTO"))
                returnedValue = returnedValue.Replace("DTO", string.Empty);
            return returnedValue;
        }

        public class GenericFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                var type = context.Type;

                if (type.IsGenericType == false)
                    return;

                schema.Title = $"{type.Name[0..^2]}<{type.GenericTypeArguments[0].Name}>";
            }
        }

    }
}
