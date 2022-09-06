using Umbraco.Core.Models.PublishedContent;
using PassleSync.Core.Models.Content.Umbraco;
using System.Linq;
using System;

namespace PassleSync.Website.ViewModels
{
    public class PasslePostViewModel : PasslePost
    {
        public PasslePostViewModel(IPublishedContent content) : base(content) { }

        public int MinsRead { get => (int)Math.Round(EstimatedReadTimeInSeconds / 60f); }
        public PostAuthor Author { get => Authors.FirstOrDefault(); }
        public string FormattedDate { get => DateTime.Parse(PublishedDate).ToString("dd MMMM yyyy"); }

        public string UmbracoAuthorUrl;
        public string PassleTitle;
    }
}
