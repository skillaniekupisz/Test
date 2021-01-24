using Autofac;
using Games.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;

namespace Games.Infrastructure.Repositories.InMemory
{
    internal static class ContainerBuilderExtensions
    {
        public static void RegisterInMemoryRepository(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterAssemblyTypes(typeof(InMemoryBaseRepository<>).Assembly)
                .AsClosedTypesOf(typeof(InMemoryBaseRepository<>))
                .EnableTelemetry()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
