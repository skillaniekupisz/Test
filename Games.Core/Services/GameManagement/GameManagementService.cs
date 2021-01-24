using Games.Core.Entties.Game;
using Games.Core.Interfaces.Repositories.Games;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Services
{
    public class GameManagementService
    {
        private readonly IGameRepository _gameRepository;

        public GameManagementService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<QueryGamesPageResult> QueryGames(QueryParameters parameters, CancellationToken cancellationToken)
        {
            try
            {
                var itemTask = _gameRepository.QueryByParameters(parameters, cancellationToken);
                var countTask = _gameRepository.CountByParameters(parameters, cancellationToken);

                await Task.WhenAll(itemTask, countTask);

                var items = await itemTask;
                var count = await countTask;

                return new QueryGamesPageResult(count, parameters.PageIndex ?? 1, items);
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task<Game> Add(Game game, CancellationToken cancellationToken)
        {
            var existingGame = await _gameRepository.Get(game.Id, cancellationToken);
            if (existingGame != null) throw new Exception();

            await _gameRepository.Add(game, cancellationToken);

            return game;
        }

        public async Task<Game> Update(Game game, CancellationToken cancellationToken)
        {
            var existingGame = await _gameRepository.Get(game.Id, cancellationToken);
            if (existingGame != null)
            {
                existingGame.CopyFrom(game);
                await _gameRepository.Add(game, cancellationToken);
            }

            return existingGame;
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var existingGame = await _gameRepository.Get(id, cancellationToken);
            if (existingGame != null)
            {
                await _gameRepository.Delete(id, cancellationToken);
            }
        }
    }
}
