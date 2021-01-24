using System.ComponentModel.DataAnnotations;

namespace Games.Api.Models.Authentication
{
    public class AuthenticationParameters
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
