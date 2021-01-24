using System;

namespace Games.Core.Services.UserManagement
{
    public class UpdateRoleParameters
    {
        public UpdateRoleParameters(string id, string[] roles)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public string Id { get; }
        public string[] Roles { get; }
    }
}
