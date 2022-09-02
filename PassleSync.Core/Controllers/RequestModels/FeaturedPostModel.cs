namespace PassleSync.Core.Controllers.RequestModels
{
    public class FeaturedPostModel
    {
        public string PostShortcode { get; set; }
        public bool IsFeaturedOnPasslePage { get; set; }
        public bool IsFeaturedOnPostPage { get; set; }
    }
}
