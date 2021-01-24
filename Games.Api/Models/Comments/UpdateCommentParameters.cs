using System.ComponentModel.DataAnnotations;

namespace Games.Api.Models.Comments
{
    public class UpdateCommentParameters
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string GameId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public decimal? Rating { get; set; }
    }
}
