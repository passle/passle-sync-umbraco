using PassleSync.Core.API.Models;
using PassleSync.Core.Constants;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public partial class PostAuthor : IBasicAuthorDetails
    {
        public string RoleInfo => Role;

        public string GetAvatarUrl(string fallbackUrl = PassleStrings.DEFAULT_AVATAR_URL)
        {
            return string.IsNullOrEmpty(ImageUrl) ? fallbackUrl : ImageUrl;
        }
    }
}
