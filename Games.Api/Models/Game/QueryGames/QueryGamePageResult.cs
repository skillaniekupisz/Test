using System.Collections.Generic;
using System.Linq;

namespace Games.Api.Models.Game
{
    public class QueryGamePageResult
    {
        public long Count { get; set; }
        public int PageIndex { get; set; }
        public IList<QueryGameResult> Items { get; set; }

        public static QueryGamePageResult Create(Core.Services.QueryGamesPageResult result)
        {
            return new QueryGamePageResult
            {
                Items = result.ITems.Select(x => QueryGameResult.Create(x)).ToList(),
                Count = result.Count,
                PageIndex = result.PageIndex
            };
        }
    }
}
