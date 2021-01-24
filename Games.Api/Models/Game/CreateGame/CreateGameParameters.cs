using System;
using System.ComponentModel.DataAnnotations;

namespace Games.Api.Models.Game
{
    public class CreateGameParameters
    {

        [Required]
        public string Name { get;  set; }

        [Required]
        public string Description { get;  set; }

        [Required]
        public string Category { get;  set; }

        [Required]
        public string ImageUrl { get;  set; }
        public DateTime ReleaseDate { get; private set; }
    }
}
