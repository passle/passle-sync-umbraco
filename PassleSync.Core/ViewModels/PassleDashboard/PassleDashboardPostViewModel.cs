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
            Title = from.GetValueOrDefault<string>("PostTitle");
            Shortcode = from.GetValueOrDefault<string>("PostShortcode");
            PassleShortcode = from.GetValueOrDefault<string>("PassleShortcode");
            Excerpt = from.GetValueOrDefault<string>("ContentTextSnippet");
            PostUrl = from.GetValueOrDefault<string>("PostUrl");
            ImageUrl = from.GetValueOrDefault<string>("ImageUrl");
            PublishedDate = DateTime.Parse(from.GetValueOrDefault<string>("PublishedDate"));
            Authors = from.GetValueOrDefault<string>("Authors");
            Tags = from.GetValueOrDefault<string>("Tags");
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
