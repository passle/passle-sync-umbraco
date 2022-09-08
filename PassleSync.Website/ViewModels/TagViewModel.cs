using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Tag = PassleSync.Core.Models.Content.Umbraco.Tag;

namespace PassleSync.Website.ViewModels
{
    public class TagViewModel : Tag
    {
        public TagViewModel(IPublishedContent content) : base(content) { }

        public string Tag;
        public IEnumerable<PasslePostViewModel> Posts;
    }
}
