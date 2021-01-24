using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Core.Entties.User
{
    public class User : EntityBase
    {
        private List<UserRole> _roles;
        public User(string username, string firstName, string lastName, string email, string password, DateTimeOffset createDate, UserRole role = null)
        {
            Id = Guid.NewGuid().ToString("N");
            Username = username ?? throw new ArgumentNullException(nameof(username));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            CreateDate = createDate;
            Roles = role == null ? new List<UserRole>() : new List<UserRole>() { role };
        }

        public string Username { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTimeOffset CreateDate { get; }
        public string Password { get; }
        public IList<UserRole> Roles
        {
            get => _roles.AsReadOnly();
            private set => _roles = value.ToList();
        }

        internal bool AddRole(UserRole role)
        {
            _roles = new List<UserRole> { role };

            return true;
        }
    }
}
