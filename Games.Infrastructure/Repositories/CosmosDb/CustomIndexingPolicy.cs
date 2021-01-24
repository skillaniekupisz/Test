using Microsoft.Azure.Cosmos;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    public class CustomIndexingPolicy
    {
        public IndexingMode Mode { get; set; } = IndexingMode.Consistent;

        public bool Autmatic { get; set; } = true;
        public string[] IncludedPaths { get; set; } = new string[] { "/*" };
        public string[] ExcludedPaths { get; set; } = new string[0];

        public static CustomIndexingPolicy None => new CustomIndexingPolicy { Mode = IndexingMode.None };
    }
}
