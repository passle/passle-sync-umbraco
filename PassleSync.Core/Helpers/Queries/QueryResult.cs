using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace PassleSync.Core.Helpers.Queries
{
    public class QueryResult<T> where T : PublishedContentModel
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage;
        public int ItemsPerPage;
        public int TotalItems;
    }
}
