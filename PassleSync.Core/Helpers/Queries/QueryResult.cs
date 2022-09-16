using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace PassleSync.Core.Helpers.Queries
{
    /// <summary>
    /// The result of a query which has been executed, including the content returned by the query, and pagination details.
    /// </summary>
    public class QueryResult<T> where T : PublishedContentModel
    {
        /// <summary>
        /// The content returned by the query.
        /// </summary>
        public IEnumerable<T> Items { get; set; }
        /// <summary>
        /// The current page of paginated results.
        /// </summary>
        public int CurrentPage;
        /// <summary>
        /// The items per page of paginated results.
        /// </summary>
        public int ItemsPerPage;
        /// <summary>
        /// The total number of items before pagination was applied.
        /// </summary>
        public int TotalItems;
        /// <summary>
        /// The total number of pages of paginated results.
        /// </summary>
        public int TotalPages;
    }
}
