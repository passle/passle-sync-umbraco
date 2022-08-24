using PassleSync.Core.Extensions;
using PassleSync.Core.Models;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardPostViewModel
    {
        public string Title;
        public string Body;
        public DateTime PublishedDate;
        public string Shortcode;
        public string PassleShortcode;
        public string Authors;
        public bool IsRepost;
        public int EstimatedReadTime;
        public string Tags;
        public string PostUrl;
        public string ImageUrl;
        public string FeaturedItemHTML;
        public string Excerpt;
        public int Id;
        public bool Synced;

        public PassleDashboardPostViewModel(IContent from)
        {
            Title = from.GetValueOrDefault<string>("PostTitle");
            Body = from.GetValueOrDefault<string>("PostBody");
            PublishedDate = from.GetValueOrDefault<DateTime>("PublishedDate");
            Shortcode = from.GetValueOrDefault<string>("PostShortcode");
            PassleShortcode = from.GetValueOrDefault<string>("PassleShortcode");
            Authors = from.GetValueOrDefault<string>("Authors");
            IsRepost = from.GetValueOrDefault<bool>("IsRepost");
            EstimatedReadTime = from.GetValueOrDefault<int>("EstimatedReadTimeInSeconds");
            Tags = from.GetValueOrDefault<string>("Tags");
            //PostUrl = from?.Url() ?? "";
            ImageUrl = from.GetValueOrDefault<string>("ImageURL");
            FeaturedItemHTML = from.GetValueOrDefault<string>("FeaturedItemHTML");
            Excerpt = from.GetValueOrDefault<string>("ContentTextSnippet");
            Id = from?.Id ?? 0;
            Synced = true;
        }

        public PassleDashboardPostViewModel(PasslePost from)
        {
            Title = from.PostTitle;
            Body = from.PostContentHtml;
            PublishedDate = DateTime.Parse(from.PublishedDate);
            Shortcode = from.PostShortcode;
            PassleShortcode = from.PassleShortcode;
            Authors = string.Join(", ", from.Authors.Select(a => a.Name));
            IsRepost = from.IsRepost;
            EstimatedReadTime = from.EstimatedReadTimeInSeconds;
            Tags = string.Join(", ", from.Tags);
            PostUrl = from.PostUrl;
            ImageUrl = from.ImageUrl;
            Excerpt = from.ContentTextSnippet;
            FeaturedItemHTML = from.FeaturedItemHtml;
            Synced = false;
        }
    }
}
