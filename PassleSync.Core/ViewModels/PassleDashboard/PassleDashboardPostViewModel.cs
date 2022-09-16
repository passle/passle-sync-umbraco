using PassleSync.Core.Models.Content.PassleApi;
using System;
using System.Linq;
using Umbraco.Core.Models;

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

        public PassleDashboardPostViewModel(IContent from)
        {
            // TODO: Can we use reflection or constants to make this more robust than magic strings?

            Id = from?.Id ?? 0;
            Title = from.GetValue<string>("postTitle");
            Shortcode = from.GetValue<string>("postShortcode");
            PassleShortcode = from.GetValue<string>("passleShortcode");
            Excerpt = from.GetValue<string>("contentTextSnippet");
            PostUrl = from.GetValue<string>("postUrl");
            ImageUrl = from.GetValue<string>("imageUrl");
            PublishedDate = from.GetValue<DateTime>("publishedDate");
            Authors = from.GetValue<string>("authors");
            Tags = from.GetValue<string>("tags");
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
