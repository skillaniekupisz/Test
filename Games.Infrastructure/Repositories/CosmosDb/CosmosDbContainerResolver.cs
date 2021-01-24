using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    public class CosmosDbContainerResolver
    {
        private readonly Database _database;
        private readonly Dictionary<string, Container> _containers;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores;

        public CosmosDbContainerResolver(JsonSerializerSettings serializerSettings, CosmosDbRepositoriesOptions options)
        {
            if (serializerSettings == null) throw new ArgumentOutOfRangeException(nameof(serializerSettings));
            if (options == null) throw new ArgumentOutOfRangeException(nameof(options));

            var clientOptions = new CosmosClientOptions
            {
                Serializer = new CosmosJsonNetSerialiser(serializerSettings)
            };

            var clinet = new CosmosClient(options.ConnectionString, clientOptions);
            _database = clinet.GetDatabase(options.Database);
            _containers = new Dictionary<string, Container>();
            _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
        }


        public async Task<Container> Resolve(string containerId, string paritionKEyPath, CustomIndexingPolicy indexingPolicy, CancellationToken cancellationToken)
        {
            if (_containers.TryGetValue(containerId, out var container))
                return container;

            var semaphore = _semaphores.GetOrAdd(containerId, x => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync(cancellationToken);

            try
            {

                if (_containers.TryGetValue(containerId, out container))
                    return container;

                var containerProperties = new ContainerProperties
                {
                    Id = containerId,
                    PartitionKeyPath = paritionKEyPath,
                    IndexingPolicy = CovertToCosmosIndexingPolicy(indexingPolicy)
                };

                _containers[containerId] = await _database.CreateContainerAsync(containerProperties, cancellationToken: cancellationToken);
                return _containers[containerId];
            }
            finally
            {
                semaphore.Release();
            }


        }

        private IndexingPolicy CovertToCosmosIndexingPolicy(CustomIndexingPolicy indexingPolicy)
        {

            var cosmosIndexingPolicy = new IndexingPolicy
            {
                IndexingMode = indexingPolicy.Mode,
                Automatic = indexingPolicy.Mode == IndexingMode.Consistent ? indexingPolicy.Autmatic : false
            };

            if (indexingPolicy.Mode == IndexingMode.None)
                return cosmosIndexingPolicy;

            foreach (var includedPath in indexingPolicy.IncludedPaths)
            {
                cosmosIndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = includedPath });
            }

            foreach (var excludedPath in indexingPolicy.ExcludedPaths)
            {
                cosmosIndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = excludedPath });
            }


            return cosmosIndexingPolicy;
        }

        private class CosmosJsonNetSerialiser : CosmosSerializer
        {
            private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
            private readonly JsonSerializer _serializer;

            public CosmosJsonNetSerialiser(JsonSerializerSettings serializerSettings)
            {
                if (serializerSettings == null) throw new ArgumentNullException(nameof(serializerSettings));

                _serializer = JsonSerializer.Create(serializerSettings);
            }

            public override T FromStream<T>(Stream stream)
            {
                using (stream)
                {
                    if (typeof(Stream).IsAssignableFrom(typeof(T)))
                    {
                        return (T)(object)(stream);
                    }

                    using var sr = new StreamReader(stream);
                    using var jsonTextReader = new JsonTextReader(sr);
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }

            public override Stream ToStream<T>(T input)
            {
                var streamPayload = new MemoryStream();
                using var streamWriter = new StreamWriter(streamPayload, DefaultEncoding, 1024, true);
                using var writer = new JsonTextWriter(streamWriter) { Formatting = Formatting.None };
                _serializer.Serialize(writer, input);
                writer.Flush();
                streamWriter.Flush();

                streamPayload.Position = 0;

                return streamPayload;
            }
        }
    }
}
