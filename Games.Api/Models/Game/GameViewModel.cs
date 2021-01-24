using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Api.Models.Game
{
    public class GameViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal AvarageRating { get; set; }
        public IList<Comment> Comments { get; set; }

        public static GameViewModel Create(Core.Entties.Game.Game game)
        {
            return game == null ? null : new GameViewModel
            {
                Id = game.Id,
                AvarageRating = game.AvarageRating,
                Category = game.Category,
                Description = game.Description,
                Comments = game.Comments.Select(x => Comment.Create(x)).ToList(),
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                ReleaseDate = game.ReleaseDate
            };
        }
    }
}
