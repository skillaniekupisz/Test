using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    public static class FeedIteratorExtensions
    {
        public static async IAsyncEnumerable<T> ReadAll<T>(this FeedIterator<T> iterator,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
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

        public static async Task<T> ReadFirstOrDefault<T>(this FeedIterator<T> iterator, CancellationToken cancellationToken)
        {
            var ru = 0.0;
            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync(cancellationToken);
                ru += results.RequestCharge;

                foreach (var result in results)
                {
                    Activity.Current?.AddTag("RU", ru.ToString(CultureInfo.InvariantCulture));
                    return result;
                }
            }

            return default;
        }
    }
}
