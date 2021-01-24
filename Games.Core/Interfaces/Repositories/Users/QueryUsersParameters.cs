namespace Games.Core.Interfaces.Repositories.Users
{
    public class QueryUsersParameters
    {
        public QueryUsersParameters(string firstname, string email, int? pageIndex, int? pageSize, string orderBy, bool isDescending)
        {
            Firstname = firstname;
            Email = email;
            PageIndex = pageIndex;
            PageSize = pageSize;
            OrderBy = orderBy;
            IsDescending = isDescending;
        }
        public string Firstname { get; }
        public string Email { get; }
        public int? PageIndex { get; }
        public int? PageSize { get; }
        public string OrderBy { get; }
        public bool IsDescending { get; }
    }
}
