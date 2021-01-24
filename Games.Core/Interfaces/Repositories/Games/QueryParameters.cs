using System;

namespace Games.Core.Interfaces.Repositories.Games
{
    public class QueryParameters
    {
        public QueryParameters(string name, string description, DateTime? releaseDate, string category, int? pageIndex, int? pageSize, string orderBy, bool isDescending)
        {
            Name = name;
            Description = description;
            ReleaseDate = releaseDate;
            Category = category;
            PageIndex = pageIndex;
            PageSize = pageSize;
            OrderBy = orderBy;
            IsDescending = isDescending;
        }

        public string Name { get; }
        public string Description { get; }
        public DateTime? ReleaseDate { get; }
        public string Category { get; }
        public int? PageIndex { get; }
        public int? PageSize { get; }
        public string OrderBy { get; }
        public bool IsDescending { get; }
    }
}
