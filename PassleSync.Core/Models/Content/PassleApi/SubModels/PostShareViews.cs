using PassleSync.Core.Attributes;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [NestedContentNameTemplate("{{socialNetwork}}")]
    public class PostShareViews
    {
        public PostShareButtons SocialNetwork { get; set; }
        public int TotalViews { get; set; }
    }
}
