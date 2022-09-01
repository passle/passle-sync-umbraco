using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using System.Linq;

namespace PassleSync.Core.Helpers
{
    public static class PostHelper
    {
        public static string LinkedInShareUrl(string postUrl, string postTitle)
        {
            return string.Format("https://www.linkedin.com/sharing/share-offsite?mini=true&url={0}&title={1}&source=LinkedIn", postUrl.UrlEncode(), postTitle.UrlEncode());
        }

        public static string TwitterShareUrl(string postUrl, string postTitle, IEnumerable<string> tags, IEnumerable<PostAuthor> authors)
        {
            string authorHashTags = string.Join(", ", authors.Select(a => string.IsNullOrEmpty(a.TwitterScreenName) ? a.PassleName : $"@{a.TwitterScreenName}"));
            string text = string.Format("{0} by {1}", postTitle, authorHashTags);
            string postHashTags = string.Join(",", tags.Select(t => t.Replace(" ", "").Replace("-", "")));
            return string.Format("https://twitter.com/intent/tweet?text={0}&url={1}&hashtags={2}", text.UrlEncode(), postUrl.UrlEncode(), postHashTags.UrlEncode());
        }

        public static string FacebookShareUrl(string postUrl, string postTitle)
        {
            return string.Format("https://www.facebook.com/sharer.php?u={0}&quote={1}", postUrl.UrlEncode(), postTitle.UrlEncode());
        }

        public static string XingShareUrl(string postUrl)
        {
            return string.Format("https://www.xing.com/spi/shares/new?url={0}", postUrl.UrlEncode());
        }
    }
}
