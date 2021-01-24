using Games.Core.Entties.User;
using Games.Core.Interfaces.Repositories.Users;
using Games.Infrastructure.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.CosmosDb.Implementations
{
    internal class CosmosDbUserRepository : CosmosDbBaseRepository<User>, IUserRepository
    {
        public CosmosDbUserRepository(CosmosDbContainerResolver containerResolver, CosmosDbRepositoriesOptions options) : base(containerResolver, options.UsersContainer,
         options.UsersPartitionKey, item => item.Id, options.UsersIndexingPolicy)
        {
        }

        [TrackDependency]
        public virtual async Task<int> CountByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var query = CosmosDbQueryBuilder<User>.GetQuery("value count(1)", container.Id)
                .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Firstname), "c.firstName = {0}", nameof(parameters.Firstname), parameters.Firstname)
                .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Email), "c.email = {0}", nameof(parameters.Email), parameters.Email);
            var iterator = container.GetItemQueryIterator<int>(query);

            return await iterator.ReadFirstOrDefault(cancellationToken);
        }

        [TrackDependency]
        public virtual async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var query = CosmosDbQueryBuilder<User>.GetQuery(container.Id).Append("c.email = {0}", nameof(email), email);
            var iterator = container.GetItemQueryIterator<User>(query);

            return await iterator.ReadFirstOrDefault(cancellationToken);
        }

        [TrackDependency]
        public virtual async Task<User> GetByUsername(string username, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var query = CosmosDbQueryBuilder<User>.GetQuery(container.Id)
                .Append("c.username = {0}", nameof(username), username);
            var iterator = container.GetItemQueryIterator<User>(query);

            return await iterator.ReadFirstOrDefault(cancellationToken);
        }

        [TrackDependency]
        public virtual async Task<User> GetByUsernameAndPassword(string username, string password, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var query = CosmosDbQueryBuilder<User>.GetQuery(container.Id)
                .Append("c.username = {0}", nameof(username), username)
                .Append("c.password = {0}", nameof(password), password);
            var iterator = container.GetItemQueryIterator<User>(query);

            return await iterator.ReadFirstOrDefault(cancellationToken);
        }

        [TrackDependency]
        public virtual async Task<IList<User>> QueryByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var query = CosmosDbQueryBuilder<User>.GetQuery(container.Id)
                .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Firstname), "c.firstName = {0}", nameof(parameters.Firstname), parameters.Firstname)
                .AppendIf(() => !string.IsNullOrWhiteSpace(parameters.Email), "c.email = {0}", nameof(parameters.Email), parameters.Email)
                .GetPage(parameters.PageSize, parameters.PageIndex);
            var iterator = container.GetItemQueryIterator<User>(query);

            return await iterator.ReadAll(cancellationToken).ToListAsync();
        }
    }
}
