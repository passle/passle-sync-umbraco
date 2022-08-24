namespace PassleSync.Core.Models
{
    public class PaginatedResponseBase
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
