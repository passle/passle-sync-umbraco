using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardPostViewModel
    {
        public int Id;
        public string Title;
        public string Shortcode;
        public string PassleShortcode;
        public string Excerpt;
        public string PostUrl;
        public string ImageUrl;
        public DateTime PublishedDate;
        public string Authors;
        public string Tags;
        public bool Synced;

        public PassleDashboardPostViewModel(IPublishedContent from)
        {
            // TODO: Can we use reflection or constants to make this more robust than magic strings?

            Id = from?.Id ?? 0;
            Title = from.Value<string>("postTitle");
            Shortcode = from.Value<string>("postShortcode");
            PassleShortcode = from.Value<string>("passleShortcode");
            Excerpt = from.Value<string>("contentTextSnippet");
            PostUrl = from.Value<string>("postUrl");
            ImageUrl = from.Value<string>("imageUrl");
            PublishedDate = from.Value<DateTime>("publishedDate");
            Authors = from.Value<string>("authors");
            Tags = from.Value<string>("tags");
            Synced = true;
        }

        public PassleDashboardPostViewModel(PasslePost from)
        {
            Id = -1;
            Title = from.PostTitle;
            Shortcode = from.PostShortcode;
            PassleShortcode = from.PassleShortcode;
            Excerpt = from.ContentTextSnippet;
            PostUrl = from.PostUrl;
            ImageUrl = from.ImageUrl;
            PublishedDate = from.PublishedDate;
            Authors = string.Join(", ", from.Authors.Select(a => a.Name));
            Tags = string.Join(", ", from.Tags);
            Synced = false;
        }
    }
}
