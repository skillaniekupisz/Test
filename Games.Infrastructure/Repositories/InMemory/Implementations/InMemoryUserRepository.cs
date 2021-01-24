using Games.Core.Entties.User;
using Games.Core.Interfaces.Repositories.Users;
using Games.Infrastructure.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.InMemory.Implementations
{
    internal class InMemoryUserRepository : InMemoryBaseRepository<User>, IUserRepository
    {
        public InMemoryUserRepository()
        {
            var admin = new User("admin", "admin", "admin", "admin", "admin", DateTimeOffset.UtcNow, new UserRole("Admin"));
            var editor = new User("editor", "editor", "editor", "editor", "editor", DateTimeOffset.UtcNow, new UserRole("Editor"));
            var user = new User("user", "user", "user", "user", "user", DateTimeOffset.UtcNow, new UserRole("User"));

            Init(new List<User> { admin, editor, user });
        }

        [TrackDependency]
        public virtual async Task<int> CountByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            var result = Items
               .Values
               .AsQueryable()
               .Where(() => !string.IsNullOrEmpty(parameters.Firstname), x => x.FirstName.Contains(parameters.Firstname, StringComparison.InvariantCultureIgnoreCase))
               .Where(() => !string.IsNullOrEmpty(parameters.Email), x => x.Email.Contains(parameters.Email, StringComparison.InvariantCultureIgnoreCase))
               .Count();

            return await Task.FromResult(result);
        }

        [TrackDependency]
        public virtual async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var user = Items.Values.AsQueryable().FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            return await Task.FromResult(user);
        }

        [TrackDependency]
        public virtual async Task<User> GetByUsernameAndPassword(string username, string password, CancellationToken cancellationToken)
        {
            var item = Items.Values.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(password));

            return await Task.FromResult(item);
        }

        [TrackDependency]
        public virtual async Task<IList<User>> QueryByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            var orderByProperty = parameters.OrderBy != null
             ? OrderByPropertues.FirstOrDefault(x => x.Equals(parameters.OrderBy, StringComparison.InvariantCultureIgnoreCase))
             : default;

            var result = Items
                .Values
                .AsQueryable()
                .Where(() => !string.IsNullOrEmpty(parameters.Firstname), x => x.FirstName.Contains(parameters.Firstname, StringComparison.InvariantCultureIgnoreCase))
                .Where(() => !string.IsNullOrEmpty(parameters.Email), x => x.Email.Contains(parameters.Email, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(orderByProperty, parameters.IsDescending)
                .GetPage(parameters.PageSize, parameters.PageIndex);

            return await Task.FromResult(result);
        }

        [TrackDependency]
        public virtual async Task<User> GetByUsername(string username, CancellationToken cancellationToken)
        {
            var user = Items.Values.AsQueryable().FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

            return await Task.FromResult(user);
        }

        private readonly string[] OrderByPropertues = new string[] { "FirstName" };
    }
}
