using System;

namespace PassleSync.Core.Models.Admin
{
    public class Post
    {
        public string PostContentHtml { get; set; }
        public string FeaturedItemHtml { get; set; }
        public int FeaturedItemPosition { get; set; }
        public string QuoteText { get; set; }
        public string QuoteUrl { get; set; }
        public bool IsFeaturedOnPasslePage { get; set; }
        public bool IsFeaturedOnPostPage { get; set; }
        public string PostShortcode { get; set; }
        public string PassleShortcode { get; set; }
        public string PostUrl { get; set; }
        public string PostTitle { get; set; }
        public object Authors { get; set; }
        public object CoAuthors { get; set; }
        public object ShareViews { get; set; }
        public string ContentTextSnippet { get; set; }
        public DateTime PublishedDate { get; set; }
        public object Tags { get; set; }
        public int FeaturedItemMediaType { get; set; }
        public int FeaturedItemEmbedType { get; set; }
        public string FeaturedItemEmbedProvider { get; set; }
        public string ImageUrl { get; set; }
        public int TotalShares { get; set; }
        public bool IsRepost { get; set; }
        public int EstimatedReadTimeInSeconds { get; set; }
        public int TotalLikes { get; set; }
        public bool OpensInNewTab { get; set; }
        public object Tweets { get; set; }
    }
}
