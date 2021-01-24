using System.Collections.Generic;
using System.Linq;

namespace Games.Api.Models.User.QueryUsers
{
    public class QueryUsersPageResult
    {
        public long Count { get; set; }
        public int PageIndex { get; set; }
        public IList<UserViewModel> Items { get; set; }

        public static QueryUsersPageResult Create(Core.Services.UserManagement.QueryUsersPageResult result)
        {
            return new QueryUsersPageResult
            {
                Count = result.Count,
                Items = result.ITems.Select(x => UserViewModel.Create(x)).ToList(),
                PageIndex = result.PageIndex,
            };
        }
    }
}
