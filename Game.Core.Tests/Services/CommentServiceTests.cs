using Games.Core.Entties.Game;
using Games.Core.Interfaces.Repositories.Games;
using Games.Core.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Game.Core.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly CommentService _commentService;
        private readonly Mock<IGameRepository> _gameRepositoryMock;

        public CommentServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _commentService = new CommentService(_gameRepositoryMock.Object);
        }

        [Fact]
        public async Task AddCommentShouldAddCommentWhenGameExists()
        {
            //arrange
            var existingGame = new Games.Core.Entties.Game.Game(Guid.NewGuid().ToString("N"), "test", "Test", "test", "Test", DateTime.UtcNow);
            _gameRepositoryMock.Setup(x => x.Get(existingGame.Id, CancellationToken.None)).ReturnsAsync(existingGame);
            var comment = new Comment("test", "test", "test", DateTime.UtcNow);

            //act
            var result = await _commentService.AddComment(existingGame.Id, comment, CancellationToken.None);

            //assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Games.Core.Entties.Game.Game>(), CancellationToken.None), Times.Once);
        }


        [Fact]
        public async Task AddCommentShouldIgnoretWhenGameNotExists()
        {
            //arrange
            var notExistingId = "notExisitingId";
            _gameRepositoryMock.Setup(x => x.Get(notExistingId, CancellationToken.None)).ReturnsAsync(default(Games.Core.Entties.Game.Game));
            var comment = new Comment("test", "test", "test", DateTime.UtcNow);

            //act
            var result = await _commentService.AddComment(notExistingId, comment, CancellationToken.None);

            //assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Games.Core.Entties.Game.Game>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task UpdateCommentShouldUpdateCommentWhenGameExists()
        {
            //arrange
            var existingGame = new Games.Core.Entties.Game.Game(Guid.NewGuid().ToString("N"), "test", "Test", "test", "Test", DateTime.UtcNow);
            _gameRepositoryMock.Setup(x => x.Get(existingGame.Id, CancellationToken.None)).ReturnsAsync(existingGame);
            var comment = new Comment("test", "test", "test", DateTime.UtcNow);

            //act
            var result = await _commentService.AddComment(existingGame.Id, comment, CancellationToken.None);

            //assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Games.Core.Entties.Game.Game>(), CancellationToken.None), Times.Once);
        }


        [Fact]
        public async Task UpdateCommentShouldIgnoretWhenGameNotExists()
        {
            //arrange
            var notExistingId = "notExisitingId";
            _gameRepositoryMock.Setup(x => x.Get(notExistingId, CancellationToken.None)).ReturnsAsync(default(Games.Core.Entties.Game.Game));
            var comment = new Comment("test", "test", "test", DateTime.UtcNow);

            //act
            var result = await _commentService.AddComment(notExistingId, comment, CancellationToken.None);

            //assert
            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Games.Core.Entties.Game.Game>(), CancellationToken.None), Times.Never);
        }
    }
}
