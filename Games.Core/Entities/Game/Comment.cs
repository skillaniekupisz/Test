using System;

namespace Games.Core.Entties.Game
{
    public class Comment
    {
        public Comment(string id, string content, string author, DateTime date, decimal rate)
        {
            Id = id;
            Content = content;
            Author = author;
            Date = date;
            Rate = rate;
        }

        public string Id { get; }
        public string Content { get; private set; }
        public string Author { get; }
        public decimal Rate { get; private set; }
        public DateTime Date { get; private set; }

        internal void CopyFrom(Comment comment)
        {
            Content = comment.Content;
            Rate = comment.Rate;
        }
    }
}
