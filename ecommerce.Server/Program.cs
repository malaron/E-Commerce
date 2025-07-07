
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ECommerce.Server.Data;

namespace ECommerce.Server
{
    public class Program
    {
        static string ConnectionString { get; set; } = null!;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

            ConfigureServices(builder.Services);

            WebApplication app = builder.Build();

            ConfigureApp(app);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();


            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(ConnectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce", Version = "v1" });
            });

            services.AddEndpointsApiExplorer();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
        }

        public static void ConfigureApp(WebApplication app)
        {
            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "my_books v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

        }
    }
}
