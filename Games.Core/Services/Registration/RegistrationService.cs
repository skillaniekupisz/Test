using Games.Core.Entties.User;
using Games.Core.Exceptions;
using Games.Core.Interfaces.Repositories.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Services.Registration
{
    public class RegistrationService
    {
        private readonly IUserRepository _userRepository;

        public RegistrationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task Register(RegisterParameters parameters, CancellationToken cancellationToken)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var user = new User(parameters.Username, parameters.FirstName, parameters.LastName, parameters.Email, parameters.Password, DateTimeOffset.UtcNow, new UserRole("user"));
            var existingUSer = await _userRepository.GetByEmail(parameters.Email, cancellationToken);
            if (existingUSer != null)
            {
                throw new ValidationException("The email already exists.");
            }

            existingUSer = await _userRepository.GetByUsername(parameters.Username, cancellationToken);
            if (existingUSer != null)
            {
                throw new ValidationException("Username already exists.");
            }

            await _userRepository.Add(user, cancellationToken);
        }
    }
}
