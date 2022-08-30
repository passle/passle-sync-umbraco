using PassleSync.Core.Attributes;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [NestedContentNameTemplate("Tweet by @{{screenName}}")]
    public class PostTweet
    {
        public string EmbedCode { get; set; }
        public string TweetId { get; set; }
        public string ScreenName { get; set; }
    }
}
