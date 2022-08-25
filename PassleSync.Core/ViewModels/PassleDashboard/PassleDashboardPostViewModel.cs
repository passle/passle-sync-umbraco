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
            Id = from?.Id ?? 0;
            Title = from.GetValueOrDefault<string>(PasslePost.PostTitleProperty);
            Shortcode = from.GetValueOrDefault<string>(PasslePost.PostShortcodeProperty);
            PassleShortcode = from.GetValueOrDefault<string>(PasslePost.PassleShortcodeProperty);
            Excerpt = from.GetValueOrDefault<string>(PasslePost.ContentTextSnippetProperty);
            PostUrl = from.GetValueOrDefault<string>(PasslePost.PostUrlProperty);
            ImageUrl = from.GetValueOrDefault<string>(PasslePost.ImageUrlProperty);
            PublishedDate = from.GetValueOrDefault<DateTime>(PasslePost.PublishedDateProperty);
            Authors = from.GetValueOrDefault<string>(PasslePost.AuthorsProperty);
            Tags = from.GetValueOrDefault<string>(PasslePost.TagsProperty);
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
            PublishedDate = DateTime.Parse(from.PublishedDate);
            Authors = string.Join(", ", from.Authors.Select(a => a.Name));
            Tags = string.Join(", ", from.Tags);
            Synced = false;
        }
    }
}
