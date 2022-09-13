using PassleSync.Core.API.Models.Conntent.PassleApi;
using PassleSync.Core.Attributes;
using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PassleAuthor : IPassleApiResponseModel
    {
        public string Name { get; set; }
        public string Shortcode { get; set; }
        public string ProfileUrl { get; set; }
        public string AvatarUrl { get; set; }
        public string RoleInfo { get; set; }
        [LongString]
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string LinkedInProfileLink { get; set; }
        public string FacebookProfileLink { get; set; }
        public string TwitterScreenName { get; set; }
        public string XingProfileLink { get; set; }
        public string SkypeProfileLink { get; set; }
        public string VimeoProfileLink { get; set; }
        public string YouTubeProfileLink { get; set; }
        public string StumbleUponProfileLink { get; set; }
        public string PinterestProfileLink { get; set; }
        public string InstagramProfileLink { get; set; }
        public IEnumerable<AuthorLink> PersonalLinks { get; set; }
        public string LocationDetail { get; set; }
        public string LocationCountry { get; set; }
        public string TagLineCompany { get; set; }
        public string SubscribeLink { get; set; }

        public string GetShortcode()
        {
            return Shortcode;
        }
    }
}
