using Games.Core.Services.UserManagement;
using System;

namespace Games.Api.Models.User
{
    public class QueryUsersParameters
    {
        public string Firstname { get; set; }
        public string Email { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; }

        internal Core.Interfaces.Repositories.Users.QueryUsersParameters ToDomainParameters()
        {
            return new Core.Interfaces.Repositories.Users.QueryUsersParameters(Firstname, Email, PageIndex, PageSize, OrderBy, IsDescending);
        }
    }
}
