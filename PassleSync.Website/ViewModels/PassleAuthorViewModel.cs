using Umbraco.Core.Models.PublishedContent;
using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using System.Linq;

namespace PassleSync.Website.ViewModels
{
    public class PassleAuthorViewModel : PassleAuthor
    {
        public PassleAuthorViewModel(IPublishedContent content) : base(content) { }

        public IEnumerable<PasslePostViewModel> AuthorPosts;
        public string FirstName { get => PassleName.Split(' ').FirstOrDefault(); }
        public string Location
        {
            get => string.Format(
                "{0}{1}{2}",
                LocationDetail,
                !string.IsNullOrEmpty(LocationCountry) && !string.IsNullOrEmpty(LocationDetail) ? ", " : "",
                LocationCountry
            );
        }

        public string SubscribeUrl;
    }
}
