using PassleSync.Core.API.Actions;

namespace PassleSync.Core.Actions
{
    public class UpdateFeaturedPostActionModel : IActionModel
    {
        public string Shortcode;
        public bool IsFeaturedOnPasslePage;
        public bool IsFeaturedOnPostPage;

        public UpdateFeaturedPostActionModel(string shortcode, string isFeaturedOnPasslePage, string isFeaturedOnPostPage)
        {
            Shortcode = shortcode;
            IsFeaturedOnPasslePage = isFeaturedOnPasslePage == "True";
            IsFeaturedOnPostPage = isFeaturedOnPostPage == "True";
        }   
    }
}
