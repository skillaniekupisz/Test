using Games.Core.Interfaces.Repositories.Games;
using System;

namespace Games.Api.Models.Game
{
    public class QueryGameParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Category { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; }

        public QueryParameters ToDomainParameters()
        {
            return new QueryParameters(Name, Description, ReleaseDate, Category, PageIndex, PageSize, OrderBy, IsDescending);
        }
    }
}
