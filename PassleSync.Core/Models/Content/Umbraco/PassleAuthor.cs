using PassleSync.Core.API.Models;
using PassleSync.Core.API.Services;
using PassleSync.Core.Constants;
using PassleSync.Core.Helpers.Queries;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public partial class PassleAuthor : IBasicAuthorDetails
    {
        /// <summary>
        /// LocationFull
        /// </summary>
        public string LocationFull => (string.IsNullOrEmpty(LocationDetail) || string.IsNullOrEmpty(LocationCountry)) ? "" : $"{LocationDetail}, {LocationCountry}";

        /// <summary>
        /// Get the URL of the author avatar, with a fallback URL if the author doesn't have an avatar.
        /// </summary>
        /// <param name="fallbackUrl">An optional custom fallback URL</param>
        public string GetAvatarUrl(string fallbackUrl = PassleStrings.DEFAULT_AVATAR_URL)
        {
            return string.IsNullOrEmpty(AvatarUrl) ? fallbackUrl : AvatarUrl;
        }

        /// <summary>
        /// Get posts that the author has written.
        /// </summary>
        public QueryResult<PasslePost> GetPosts(int currentPage = 1, int itemsPerPage = 4)
        {
            var passleHelperService = Current.Factory.GetInstance<IPassleHelperService>();
            return passleHelperService.GetPosts().ByAuthorShortcode(Shortcode).WithCurrentPage(currentPage).WithItemsPerPage(itemsPerPage).Execute();
        }
    }
}
