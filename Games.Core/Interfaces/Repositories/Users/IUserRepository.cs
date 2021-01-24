using Games.Core.Entties.User;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Core.Interfaces.Repositories.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
        Task<User> GetByUsernameAndPassword(string username, string password, CancellationToken cancellationToken);
        Task Add(User user, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
        Task<IList<User>> QueryByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken);
        Task<int> CountByParameters(QueryUsersParameters parameters, CancellationToken cancellationToken);
        Task<User> Get(string id, CancellationToken cancellationToken);
        Task<IList<User>> GetByIds(IList<string> ids, CancellationToken cancellationToken);
        Task<User> GetByUsername(string username, CancellationToken cancellationToken);
    }
}
