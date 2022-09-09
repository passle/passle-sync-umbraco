using PassleSync.Core.Extensions;
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
            Title = from.GetValueOrDefault<string>("postTitle");
            Shortcode = from.GetValueOrDefault<string>("postShortcode");
            PassleShortcode = from.GetValueOrDefault<string>("passleShortcode");
            Excerpt = from.GetValueOrDefault<string>("contentTextSnippet");
            PostUrl = from.GetValueOrDefault<string>("postUrl");
            ImageUrl = from.GetValueOrDefault<string>("imageUrl");
            PublishedDate = DateTime.Parse(from.GetValueOrDefault<string>("publishedDate"));
            Authors = from.GetValueOrDefault<string>("authors");
            Tags = from.GetValueOrDefault<string>("tags");
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
