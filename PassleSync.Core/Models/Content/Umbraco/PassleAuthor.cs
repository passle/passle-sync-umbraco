using PassleSync.Core.API.Models;
using PassleSync.Core.Constants;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public partial class PassleAuthor : IBasicAuthorDetails
    {
        public string LocationFull => (string.IsNullOrEmpty(LocationDetail) || string.IsNullOrEmpty(LocationCountry)) ? "" : $"{LocationDetail}, {LocationCountry}";

        public string GetAvatarUrl(string fallbackUrl = PassleStrings.DEFAULT_AVATAR_URL)
        {
            return string.IsNullOrEmpty(AvatarUrl) ? fallbackUrl : AvatarUrl;
        }
    }
}
