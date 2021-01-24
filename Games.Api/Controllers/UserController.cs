using Games.Api.Models.User.QueryUsers;
using Games.Api.Models.User.UpdateRole;
using Games.Core.Interfaces.Repositories.Users;
using Games.Core.Services.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;

        public UserController(UserManagementService userManagementService, IUserRepository userRepository)
        {
            _userManagementService = userManagementService ?? throw new ArgumentNullException(nameof(userManagementService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.User.QueryUsers.QueryUsersPageResult>> Search([FromQuery] Models.User.QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _userManagementService.QueryUsers(parameters.ToDomainParameters(), cancellationToken);

            return Ok(Models.User.QueryUsers.QueryUsersPageResult.Create(result));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserViewModel>> Get(string id, CancellationToken cancellationToken)
        {
            var result = await _userRepository.Get(id, cancellationToken);

            return UserViewModel.Create(result);
        }

        [HttpPost("roles")]
        [Authorize]
        public async Task<ActionResult> UpdateRoles([FromBody] IEnumerable<Models.User.UpdateRole.UpdateRoleParameters> parameters, CancellationToken cancellationToken)
        {
             await _userManagementService.UpdateRoles(parameters.Select(x => x.ToDomainParamters()).ToList(), cancellationToken);

            return Ok();
        }
    }
}
