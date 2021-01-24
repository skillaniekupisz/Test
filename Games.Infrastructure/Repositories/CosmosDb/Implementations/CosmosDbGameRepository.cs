using Games.Common;
using Games.Core.Entties.Game;
using Games.Core.Interfaces.Repositories.Games;
using Games.Infrastructure.Telemetry;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.CosmosDb.Implementations
{
    internal class CosmosDbGameRepository : CosmosDbBaseRepository<Game>, IGameRepository
    {
        public CosmosDbGameRepository(CosmosDbContainerResolver containerResolver, CosmosDbRepositoriesOptions options) : base(containerResolver, options.GamesContainer,
            options.GamesPartitionKey, item => item.Id, options.GamesIndexingPolicy)
        {
        }

        [TrackDependency]
        public virtual async Task<int> CountByParameters(QueryParameters parameters, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);

            var query = CosmosDbQueryBuilder<Game>.GetQuery("VALUE COUNT(1)", container.Id)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Category), "c.category = {0}", nameof(parameters.Category), parameters.Category)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Description), "CONTAINS(c.description, {0}, true)", nameof(parameters.Description), parameters.Description)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Name), "CONTAINS(c.name, {0}, true)", nameof(parameters.Name), parameters.Name)
              .OrderByIf(() => !string.IsNullOrWhiteSpace(parameters.OrderBy), $"c.{parameters.OrderBy.ToCamelCase()}", parameters.IsDescending)
              .GetPage(parameters.PageSize, parameters.PageIndex);

            var iterator = container.GetItemQueryIterator<int>(query);

            return await iterator.ReadFirstOrDefault(cancellationToken);
        }

        [TrackDependency]
        public virtual async Task<IList<Game>> QueryByParameters(QueryParameters parameters, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);

            var query = CosmosDbQueryBuilder<Game>.GetQuery(container.Id)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Category), "c.category = {0}", nameof(parameters.Category), parameters.Category)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Description), "CONTAINS(c.description, {0}, true)", nameof(parameters.Description), parameters.Description)
              .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Name), "CONTAINS(c.name, {0}, true)", nameof(parameters.Name), parameters.Name)
              .OrderByIf(() => !string.IsNullOrWhiteSpace(parameters.OrderBy), $"c.{parameters.OrderBy.ToCamelCase()}", parameters.IsDescending)
              .GetPage(parameters.PageSize, parameters.PageIndex);

            var iterator = container.GetItemQueryIterator<Game>(query);

            return await iterator.ReadAll(cancellationToken).ToListAsync();
        }

        [TrackDependency]
        public virtual async Task<IList<Game>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAll(cancellationToken).ToListAsync();
        }
    }
}
