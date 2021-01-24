using Games.Core.Entties.Game;
using Games.Core.Interfaces.Repositories.Games;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Services
{
    public class CommentService
    {
        private readonly IGameRepository _gameRepository;

        public CommentService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> AddComment(string gameId, Comment comment, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.Get(gameId, cancellationToken);
            if (game != null)
            {
                game.AddComment(comment);
                await _gameRepository.Update(game, cancellationToken);
            }
            return game;
        }

        public async Task<Game> RemoveComment(string gameId, string commentId, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.Get(gameId, cancellationToken);
            if (game != null && game.RemoveComment(commentId))
            {
                await _gameRepository.Update(game, cancellationToken);
            }
            return game;
        }

        public async Task<Game> UpdateComment(string gameId, Comment comment, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.Get(gameId, cancellationToken);
            if (game != null && game.UpdateComment(comment))
            {
                await _gameRepository.Update(game, cancellationToken);
            }
            return game;
        }
    }
}
