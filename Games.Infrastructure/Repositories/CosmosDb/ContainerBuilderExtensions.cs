using Autofac;
using Games.Common.Configuration;
using Games.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterCosmosDbRepositories(this ContainerBuilder builder, IConfiguration configuration)
        {
            var options = configuration.GetValueOrDefault<CosmosDbRepositoriesOptions>();

            builder.RegisterInstance(options);
            builder.RegisterType<CosmosDbContainerResolver>().AsSelf();
            builder.RegisterAssemblyTypes(typeof(CosmosDbBaseRepository<>).Assembly)
                .AsClosedTypesOf(typeof(CosmosDbBaseRepository<>))
                .AsImplementedInterfaces()
                .EnableTelemetry()
                .AutoActivate()
                .SingleInstance();
        }
    }
}
