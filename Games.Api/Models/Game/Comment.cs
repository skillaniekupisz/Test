using System;

namespace Games.Api.Models.Game
{

    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }

        internal static Comment Create(Core.Entties.Game.Comment x)
        {
            return new Comment
            {
                Id = x.Id,
                Author = x.Author,
                Content = x.Content,
                Date = x.Date
            };
        }
    }
}
