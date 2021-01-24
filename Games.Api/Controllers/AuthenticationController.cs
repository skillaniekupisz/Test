using Games.Api.Authentication;
using Games.Api.Models.Authentication;
using Games.Core.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Api.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtGenerator _jwtGenerator;

        public AuthenticationController(IUserRepository userService, JwtGenerator jwtGenerator)
        {
            _userRepository = userService ?? throw new ArgumentNullException(nameof(userService));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
        }


        [HttpPost]
        public async Task<ActionResult<AuthenticationResult>> Authenticate([FromBody] AuthenticationParameters parameters, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAndPassword(parameters.Username, parameters.Password, cancellationToken);
            if (user == null)
            {
                return Unauthorized();
            }

            var accessToken = _jwtGenerator.Generator(user);

            return new AuthenticationResult { AccessToken = accessToken };
        }
    }
}
