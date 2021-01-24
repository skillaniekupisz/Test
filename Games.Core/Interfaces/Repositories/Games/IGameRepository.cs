using Games.Core.Entties.Game;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Interfaces.Repositories.Games
{
    public interface IGameRepository
    {
        Task<IList<Game>> GetAll(CancellationToken cancellationToken);
        Task Add(Game game, CancellationToken cancellationToken);
        Task Update(Game game, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
        Task<Game> Get(string id, CancellationToken cancellationToken);
        Task<IList<Game>> QueryByParameters(QueryParameters parameters, CancellationToken cancellationToken);
        Task<int> CountByParameters(QueryParameters parameters, CancellationToken cancellationToken);
    }
}
