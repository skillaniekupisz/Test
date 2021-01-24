using Games.Api.Models.Game;
using Games.Core.Interfaces.Repositories.Games;
using Games.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameManagementService _gameManagementService;
        private readonly IGameRepository _gameRepository;

        public GameController(GameManagementService gameManagementService, IGameRepository gameRepository)
        {
            _gameManagementService = gameManagementService ?? throw new ArgumentNullException(nameof(gameManagementService));
            _gameRepository = gameRepository;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GameViewModel>> Get(string id, CancellationToken cancellationToken)
        {
            var result = await _gameRepository.Get(id, cancellationToken);

            return GameViewModel.Create(result);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<QueryGamePageResult>> Search([FromQuery] QueryGameParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _gameManagementService.QueryGames(parameters.ToDomainParameters(), cancellationToken);

            return Ok(QueryGamePageResult.Create(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<GameViewModel>> Add([FromQuery] CreateGameParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _gameManagementService.Add(new Core.Entties.Game.Game(Guid.NewGuid().ToString("N"), parameters.Name, parameters.Description, parameters.Category, parameters.ImageUrl, parameters.ReleaseDate), cancellationToken);

            return Ok(GameViewModel.Create(result));
        }

        [HttpPut]
        [Authorize]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<GameViewModel>> Update([FromQuery] UpdateGameParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _gameManagementService.Update(new Core.Entties.Game.Game(parameters.Id, parameters.Name, parameters.Description, parameters.Category, parameters.ImageUrl, parameters.ReleaseDate), cancellationToken);

            return Ok(GameViewModel.Create(result));
        }

        [HttpDelete]
        [Authorize]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete([FromQuery] string id, CancellationToken cancellationToken)
        {
            await _gameManagementService.Delete(id, cancellationToken);

            return Ok();
        }
    }
}
