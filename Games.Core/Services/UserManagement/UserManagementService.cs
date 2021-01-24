using Games.Core.Interfaces.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Services.UserManagement
{
    public class UserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly string[] _roles = new string[] { "Admin", "User", "Editor" };
        public UserManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<QueryUsersPageResult> QueryUsers(QueryUsersParameters parameters, CancellationToken cancellationToken)
        {
            try
            {
                var itemTask = _userRepository.QueryByParameters(parameters, cancellationToken);
                var countTask = _userRepository.CountByParameters(parameters, cancellationToken);

                await Task.WhenAll(itemTask, countTask);

                var items = await itemTask;
                var count = await countTask;

                return new QueryUsersPageResult(count, parameters.PageIndex ?? 1, items);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateRoles(IList<UpdateRoleParameters> lists, CancellationToken cancellationToken)
        {
            var ids = lists.Select(x => x.Id).ToList();
            var users = await _userRepository.GetByIds(ids, cancellationToken);

            foreach (var user in users)
            {
                var item = lists.FirstOrDefault(x => x.Id == user.Id);
                foreach (var role in item.Roles)
                {
                    if (_roles.Contains(role))
                    {
                        user.AddRole(new Entties.User.UserRole(role));
                    }
                }

                await _userRepository.Update(user, cancellationToken);
            }
        }
    }
}
