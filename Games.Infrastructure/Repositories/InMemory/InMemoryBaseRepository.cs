using Games.Core.Entties;
using Games.Infrastructure.Telemetry;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.InMemory
{
    internal abstract class InMemoryBaseRepository<T> where T : EntityBase
    {
        protected readonly ConcurrentDictionary<string, T> Items;

        public InMemoryBaseRepository()
        {
            Items = new ConcurrentDictionary<string, T>();
        }

        [TrackDependency]
        public virtual async Task<T> Get(string id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Items.TryGetValue(id, out var value) ? value : default);
        }

        [TrackDependency]
        public virtual async Task<IList<T>> GetByIds(IList<string> ids, CancellationToken cancellationToken)
        {
            return await Items.Where(x => ids.Contains(x.Key)).Select(x => x.Value).ToAsyncEnumerable().ToListAsync();
        }

        [TrackDependency]
        public virtual async Task<IList<T>> GetAll(CancellationToken cancellationToken)
        {
            return await Items.Select(x => x.Value).ToAsyncEnumerable().ToListAsync();
        }

        [TrackDependency]
        public virtual async Task Add(T item, CancellationToken cancellationToken)
        {
            Items.TryAdd(item.Id, item);
            await Task.FromResult(item);
        }

        [TrackDependency]
        public virtual async Task Delete(string id, CancellationToken cancellationToken)
        {
            Items.TryRemove(id, out var _);
            await Task.FromResult(id);
        }

        [TrackDependency]
        public virtual async Task Update(T item, CancellationToken cancellationToken)
        {
            if (Items.TryGetValue(item.Id, out var currentValue))
            {
                Items.TryUpdate(item.Id, item, currentValue);
            }

            await Task.FromResult(cancellationToken);
        }

        protected void Init(IList<T> items)
        {
            foreach (var item in items)
            {
                Items.TryAdd(item.Id, item);
            }
        }
    }
}
