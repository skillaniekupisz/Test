using Games.Api.Authentication;
using Games.Api.Models.Comments;
using Games.Api.Models.Game;
using Games.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsContoller : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentsContoller(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GameViewModel>> Add([FromQuery] AddCommentParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _commentService.AddComment(parameters.GameId, new Core.Entties.Game.Comment(Guid.NewGuid().ToString("N"), parameters.Content, User.GetUserName(), DateTime.UtcNow, parameters.Rating.Value), cancellationToken);
            return Ok(GameViewModel.Create(result));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<GameViewModel>> Update([FromQuery] UpdateCommentParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _commentService.UpdateComment(parameters.GameId, new Core.Entties.Game.Comment(parameters.Id, parameters.Content, User.GetUserName(), DateTime.UtcNow, parameters.Rating.Value), cancellationToken);
            return Ok(GameViewModel.Create(result));
        }

        [HttpDelete("/api/game/{gameId}/comment{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string gameId, string id, CancellationToken cancellationToken)
        {
            await _commentService.RemoveComment(gameId, id, cancellationToken);
            return Ok();
        }

    }
}
