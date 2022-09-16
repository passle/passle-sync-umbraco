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
        public string LocationFull => (string.IsNullOrEmpty(LocationDetail) || string.IsNullOrEmpty(LocationCountry)) ? "" : $"{LocationDetail}, {LocationCountry}";

        public string GetAvatarUrl(string fallbackUrl = PassleStrings.DEFAULT_AVATAR_URL)
        {
            return string.IsNullOrEmpty(AvatarUrl) ? fallbackUrl : AvatarUrl;
        }

        public QueryResult<PasslePost> GetPosts(int currentPage = 1, int itemsPerPage = 10)
        {
            var passleHelperService = Current.Factory.GetInstance<IPassleHelperService>();
            return passleHelperService.GetPosts().ByAuthorShortcode(Shortcode).WithCurrentPage(currentPage).WithItemsPerPage(itemsPerPage).Execute();
        }
    }
}
