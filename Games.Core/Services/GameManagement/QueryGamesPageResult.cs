using Games.Core.Entties.Game;
using System.Collections.Generic;

namespace Games.Core.Services
{
    public class QueryGamesPageResult
    {
        public QueryGamesPageResult(long count, int pageIndex, IList<Game> iTems)
        {
            Count = count;
            PageIndex = pageIndex;
            ITems = iTems;
        }

        public long Count { get; }
        public int PageIndex { get; }
        public IList<Game> ITems { get; }
    }
}
