using Autofac;
using Games.Infrastructure.Repositories.CosmosDb;
using Games.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.Configuration;
using System;

namespace Games.Infrastructure.Repositories
{
    internal static class ContainerBuilderExtensions
    {
        public static void RegisterRepositories(this ContainerBuilder builder, IConfiguration configuration)
        {
            var type = configuration.GetValue<RepositoriesType>(nameof(RepositoriesOptions.Type));

            switch (type)
            {
                case RepositoriesType.InMemory:
                    builder.RegisterInMemoryRepository(configuration);
                    break;
                case RepositoriesType.CosmosDb:
                    builder.RegisterCosmosDbRepositories(configuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
