using System.ComponentModel.DataAnnotations;

namespace Games.Api.Models.User.UpdateRole
{
    public class UpdateRoleParameters
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string[] Roles { get; set; }

        internal Core.Services.UserManagement.UpdateRoleParameters ToDomainParamters()
        {
            return new Core.Services.UserManagement.UpdateRoleParameters(Id, Roles);
        }
    }
}
