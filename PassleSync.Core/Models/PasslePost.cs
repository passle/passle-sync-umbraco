using PassleSync.Core.Models.PassleSync;
using System.Collections.Generic;

namespace PassleSync.Core.Models
{
    public class PasslePost
    {
        public string PostTitle { get; set; }
        public string PostContentHtml { get; set; }
        public string PublishedDate { get; set; }
        public string PostShortcode { get; set; }
        public string PassleShortcode { get; set; }
        public IEnumerable<PassleAuthor> Authors { get; set; }
        public bool IsRepost { get; set; }
        public int EstimatedReadTimeInSeconds { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string PostUrl { get; set; }
        public string ImageUrl { get; set; }
        public string FeaturedItemHtml { get; set; }
        public string ContentTextSnippet { get; set; }
        public int FeaturedItemPosition { get; set; }
        public string QuoteText { get; set; }
        public string QuoteUrl { get; set; }
        public bool IsFeaturedOnPasslePage { get; set; }
        public bool IsFeaturedOnPostPage { get; set; }
        public IEnumerable<PassleAuthor> CoAuthors { get; set; }
        public IEnumerable<PassleShareViewsNetwork> ShareViews { get; set; }
        public int FeaturedItemMediaType { get; set; }
        public int FeaturedItemEmbedType { get; set; }
        public string FeaturedItemEmbedProvider { get; set; }
        public int TotalShares { get; set; }
        public int TotalLikes { get; set; }
        public bool OpensInNewTab { get; set; }
        public IEnumerable<object> Tweets { get; set; } // TODO: Don't use object type here
    }
}
