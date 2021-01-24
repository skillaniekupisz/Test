using System.Linq;

namespace Games.Api.Models.User.QueryUsers
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }

        internal static UserViewModel Create(Core.Entties.User.User x)
        {
            return new UserViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Roles = x.Roles.Select(x => x.Name).ToArray(),
                Username = x.Username,
            };
        }
    }
}
