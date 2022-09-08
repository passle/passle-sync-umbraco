using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace PassleSync.Website.ViewModels
{
    public class InsightsPageViewModel : InsightsPage
    {
        public InsightsPageViewModel(IPublishedContent content) : base(content)
        {
        }

        public IEnumerable<PasslePost> Posts;
        public PaginationViewModel Pagination;
    }
}
