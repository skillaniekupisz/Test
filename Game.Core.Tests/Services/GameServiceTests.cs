//using Games.Core.Interfaces.Repositories.Games;
//using Games.Core.Services;
//using Moq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Game.Core.Services
//{
//    public class GameServiceTests
//    {
//        private readonly GameManagementService _gameManagementService;
//        private readonly Mock<IGameRepository> _gameRepositoryMcok;

//        public GameServiceTests()
//        {
//            _gameRepositoryMcok = new Mock<IGameRepository>();
//            _gameManagementService = new GameManagementService(_gameRepositoryMcok.Object);
//        }


//        [Fact]
//        public async Task AddGameShouldAddtWhenGameNoExists()
//        {
//            //arrange
//            var existingGame = new Games.Core.Entties.Game.Game(Guid.NewGuid().ToString("N"), "test", "Test", "test", "Test", DateTime.UtcNow);
//            _gameRepositoryMock.Setup(x => x.Get(existingGame.Id, CancellationToken.None)).ReturnsAsync(existingGame);
//            var comment = new Comment("test", "test", "test", DateTime.UtcNow);

//            //act
//            var result = await _commentService.AddComment(existingGame.Id, comment, CancellationToken.None);

//            //assert
//            _gameRepositoryMock.Verify(x => x.Update(It.IsAny<Games.Core.Entties.Game.Game>(), CancellationToken.None), Times.Once);
//        }

//    }
//}
