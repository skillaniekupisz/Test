namespace Games.Infrastructure.Repositories.CosmosDb
{
    public class CosmosDbRepositoriesOptions : RepositoriesOptions
    {
        public string ConnectionString { get; set; }
        public string ApplicationRegions { get; set; } = "West Europe";
        public string Database { get; set; }

        public string GamesContainer { get; set; } = "Games";
        public string GamesPartitionKey { get; set; } = "/id";
        public CustomIndexingPolicy GamesIndexingPolicy { get; set; } = new CustomIndexingPolicy
        {
            IncludedPaths = new[]
            {
                "/name/?",
                "/description/?",
                "/category/?",
            },
            ExcludedPaths = new[] { "/*" }
        };

        public string UsersContainer { get; set; } = "Users";
        public string UsersPartitionKey { get; set; } = "/id";
        public CustomIndexingPolicy UsersIndexingPolicy { get; set; } = new CustomIndexingPolicy
        {
            IncludedPaths = new[]
            {
                "/username/?",
                "/firstName/?",
                "/password/?",
                "/email/?",
            },
            ExcludedPaths = new[] { "/*" }
        };
    }
}
