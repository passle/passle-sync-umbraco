using System;
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
        public string FeaturedItemHTML { get; set; }
        public string ContentTextSnippet { get; set; }
    }
}
