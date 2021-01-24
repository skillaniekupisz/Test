using Games.Core.Entties.User;
using System.Collections.Generic;

namespace Games.Core.Services.UserManagement
{
    public class QueryUsersPageResult
    {
        public QueryUsersPageResult(long count, int pageIndex, IList<User> iTems)
        {
            Count = count;
            PageIndex = pageIndex;
            ITems = iTems;
        }

        public long Count { get; }
        public int PageIndex { get; }
        public IList<User> ITems { get; }
    }
}
