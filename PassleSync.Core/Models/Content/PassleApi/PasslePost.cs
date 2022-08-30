using PassleSync.Core.Attributes;
using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PasslePost
    {
        public string PostTitle { get; set; }
        public string PostShortcode { get; set; }
        public string PassleShortcode { get; set; }
        public string PublishedDate { get; set; }
        public string PostUrl { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<PostAuthor> Authors { get; set; }
        public IEnumerable<PostAuthor> CoAuthors { get; set; }
        [LongString]
        public string PostContentHtml { get; set; }
        public string ContentTextSnippet { get; set; }
        [LongString]
        public string QuoteText { get; set; }
        public string QuoteUrl { get; set; }
        public bool IsRepost { get; set; }
        public bool IsFeaturedOnPasslePage { get; set; }
        public bool IsFeaturedOnPostPage { get; set; }
        public int? EstimatedReadTimeInSeconds { get; set; }
        public string FeaturedItemHtml { get; set; }
        public PostFeaturedItemPosition FeaturedItemPosition { get; set; }
        public PostMediaType FeaturedItemMediaType { get; set; }
        public PostIframelyObjectType FeaturedItemEmbedType { get; set; }
        public string FeaturedItemEmbedProvider { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<PostTweet> Tweets { get; set; }
        public IEnumerable<PostShareViews> ShareViews { get; set; }
        public int TotalShares { get; set; }
        public int TotalLikes { get; set; }
        public bool OpensInNewTab { get; set; }
    }
}
