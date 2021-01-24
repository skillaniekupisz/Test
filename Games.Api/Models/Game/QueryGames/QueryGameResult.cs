using System;

namespace Games.Api.Models.Game
{
    public class QueryGameResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal AvarageRating { get; set; }

        public static QueryGameResult Create(Core.Entties.Game.Game game)
        {
            return new QueryGameResult
            {
                Id = game.Id,
                Name = game.Name,
                AvarageRating = game.AvarageRating,
                Category = game.Category,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                ReleaseDate = game.ReleaseDate
            };
        }
    }
}
