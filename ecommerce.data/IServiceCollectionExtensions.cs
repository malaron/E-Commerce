using JasperFx;
using JasperFx.Events.Projections;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using SharedContracts;

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

                // This is part of the sample, can be removed later
                options.Projections.Add<UserProjection>(ProjectionLifecycle.Inline);

                if (isDevelopment)
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            }).UseNpgsqlDataSource();


            return serviceCollection;
        }
    }
}
