using PassleSync.Core.API.Models;
using PassleSync.Core.Constants;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public partial class PostAuthor : IBasicAuthorDetails
    {
        /// <summary>
        /// RoleInfo
        /// </summary>
        public string RoleInfo => Role;

        /// <summary>
        /// Get the URL of the author avatar, with a fallback URL if the author doesn't have an avatar.
        /// </summary>
        /// <param name="fallbackUrl">An optional custom fallback URL</param>
        public string GetAvatarUrl(string fallbackUrl = PassleStrings.DEFAULT_AVATAR_URL)
        {
            return string.IsNullOrEmpty(ImageUrl) ? fallbackUrl : ImageUrl;
        }
    }
}
