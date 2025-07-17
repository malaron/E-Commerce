using ImTools;
using JasperFx;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Marten;


namespace eCommerce.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataConnector(this IServiceCollection serviceCollection, string connectionString, bool isDevelopment)
        {
            serviceCollection.AddNpgsqlDataSource(connectionString);

            serviceCollection.AddMarten(options =>
            {
                options.UseSystemTextJsonForSerialization();

                options.Projections.Add<ProductProjection>(ProjectionLifecycle.Inline);

                if (isDevelopment)
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            }).UseNpgsqlDataSource().IntegrateWithWolverine(options =>
            {
                options.UseFastEventForwarding = true;
            });

            return serviceCollection;
        }
    }
}
