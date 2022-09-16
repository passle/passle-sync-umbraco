using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using PassleSync.Core.Models.Content.Umbraco;

namespace PassleSync.Website.ViewModels
{
    public class HomePageViewModel : HomePage
    {
        public HomePageViewModel(IPublishedContent content) : base(content) { }

        public IEnumerable<PasslePost> Posts;
        public PasslePost FeaturedPost;
        public string SearchQuery;
        public string TagFilter;
        public PaginationViewModel Pagination;
    }
}
