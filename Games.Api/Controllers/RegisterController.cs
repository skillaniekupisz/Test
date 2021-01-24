using Games.Core.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Api.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegisterController(RegistrationService registrationService)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromQuery] Models.Register.RegisterParameters parameters, CancellationToken cancellationToken)
        {
            await _registrationService.Register(parameters.ToDomainParateres(), cancellationToken);

            return Ok();
        }
    }
}
