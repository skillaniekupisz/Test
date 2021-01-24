using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Core.Entties.Game
{
    public class Game : EntityBase
    {
        private List<Comment> _comments;
        public Game(string id, string name, string description, string category, string imageUrl, DateTime releaseDate)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            ImageUrl = imageUrl;
            ReleaseDate = releaseDate;
            Comments = new List<Comment>();
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string ImageUrl { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public decimal AvarageRating => _comments.Any() ? _comments.Sum(x => x.Rate) / _comments.Count : 0;

        public IList<Comment> Comments
        {
            get => _comments.AsReadOnly();
            private set { _comments = value.ToList(); }
        }

        internal void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }

        internal bool RemoveComment(string commentId)
        {
            var comment = _comments.FirstOrDefault(x => x.Id == commentId);
            if (comment == null)
                return false;

            _comments.Remove(comment);
            return true;
        }

        internal bool UpdateComment(Comment comment)
        {
            var existingComment = _comments.FirstOrDefault(x => x.Id == comment.Id);
            if (existingComment == null)
                return false;

            existingComment.CopyFrom(comment);
            return true;
        }

        internal void CopyFrom(Game game)
        {
            Name = game.Name;
            Description = game.Description;
            Category = game.Category;
            ImageUrl = game.ImageUrl;
            ReleaseDate = game.ReleaseDate;
        }
    }
}
