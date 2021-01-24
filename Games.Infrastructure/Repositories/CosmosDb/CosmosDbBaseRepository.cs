using Games.Core.Entties;
using Games.Infrastructure.Telemetry;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    internal abstract class CosmosDbBaseRepository<T> where T : EntityBase
    {
        private readonly CosmosDbContainerResolver _containerResolver;
        private readonly string _containerId;
        private readonly string _partitionKeyPath;
        private readonly Func<T, string> _idSelector;
        private readonly CustomIndexingPolicy _indexingPolicy;

        protected CosmosDbBaseRepository(CosmosDbContainerResolver containerResolver, string containerId, string partitionKeyPath,
            Func<T, string> idSelector, CustomIndexingPolicy indexingPolicy)
        {
            _containerResolver = containerResolver ?? throw new ArgumentNullException(nameof(containerResolver));
            _containerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
            _partitionKeyPath = partitionKeyPath ?? throw new ArgumentNullException(nameof(partitionKeyPath));
            _idSelector = idSelector ?? throw new ArgumentNullException(nameof(idSelector));
            _indexingPolicy = indexingPolicy ?? throw new ArgumentNullException(nameof(indexingPolicy));
        }

        [TrackDependency]
        public virtual async Task<T> Get(string id, CancellationToken cancellationToken)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            try
            {
                var container = await GetContainer(cancellationToken);
                var itemResponse = await container.ReadItemAsync<T>(id, new PartitionKey(id), cancellationToken: cancellationToken);

                Activity.Current?.AddTag("RU", itemResponse.RequestCharge.ToString(CultureInfo.InvariantCulture));

                return itemResponse.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Activity.Current?.AddTag("RU", ex.RequestCharge.ToString(CultureInfo.InvariantCulture));
                return null;
            }
        }

        [TrackDependency]
        public virtual async Task<IList<T>> GetByIds(IList<string> ids, CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var sqlQuery = new QueryDefinition($"select * from {container.Id} c where ARRAY_CONTAINS(@ids, c.id)")
                .WithParameter("@ids", ids);

            var iterator = container.GetItemQueryIterator<T>(sqlQuery);

            return await iterator.ReadAll(cancellationToken).ToListAsync(cancellationToken);
        }

        [TrackDependency]
        public virtual async IAsyncEnumerable<T> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var container = await GetContainer(cancellationToken);
            var sqlQuery = new QueryDefinition($"select * from {container.Id} c");

            var iterator = container.GetItemQueryIterator<T>(sqlQuery);
            var ru = 0.0;

            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync(cancellationToken);
                ru += results.RequestCharge;

                foreach (var result in results)
                {
                    yield return result;
                }
            }

            Activity.Current?.AddTag("RU", ru.ToString(CultureInfo.InvariantCulture));
        }

        [TrackDependency]
        public virtual async Task Add(T item, CancellationToken cancellationToken)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            try
            {
                var container = await GetContainer(cancellationToken);
                var itemResponse = await container.CreateItemAsync(item, null, cancellationToken: cancellationToken);

                Activity.Current?.AddTag("RU", itemResponse.RequestCharge.ToString(CultureInfo.InvariantCulture));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                Activity.Current?.AddTag("RU", ex.RequestCharge.ToString(CultureInfo.InvariantCulture));

                throw;
            }
        }

        [TrackDependency]
        public virtual async Task Update(T item, CancellationToken cancellationToken)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var requestOptions = new ItemRequestOptions { IfMatchEtag = item.Etag };
            var container = await GetContainer(cancellationToken);

            try
            {
                var itemResponse = await container.ReplaceItemAsync(item, _idSelector(item), requestOptions: requestOptions, cancellationToken: cancellationToken);

                Activity.Current?.AddTag("RU", itemResponse.RequestCharge.ToString(CultureInfo.InvariantCulture));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                Activity.Current?.AddTag("RU", ex.RequestCharge.ToString(CultureInfo.InvariantCulture));

                throw;
            }
        }

        [TrackDependency]
        public virtual async Task Delete(string id, CancellationToken cancellationToken)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var container = await GetContainer(cancellationToken);

            try
            {
                var itemResponse = await container.DeleteItemAsync<T>(id, new PartitionKey(id), cancellationToken: cancellationToken);
                Activity.Current?.AddTag("RU", itemResponse.RequestCharge.ToString(CultureInfo.InvariantCulture));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Activity.Current?.AddTag("RU", ex.RequestCharge.ToString(CultureInfo.InvariantCulture));

                throw;
            }
        }

        internal Task<Container> GetContainer(CancellationToken cancellationToken) =>
            _containerResolver.Resolve(_containerId, _partitionKeyPath, _indexingPolicy, cancellationToken);
    }
}
