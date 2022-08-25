using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PassleAuthor
    {
        public static string NameProperty { get => "AuthorName"; }
        public string Name { get; set; }
        public static string ShortcodeProperty { get => "Shortcode"; }
        public string Shortcode { get; set; }
        public static string ProfileUrlProperty { get => "ProfileUrl"; }
        public string ProfileUrl { get; set; }
        public static string AvatarUrlProperty { get => "AvatarUrl"; }
        public string AvatarUrl { get; set; }
        public static string RoleInfoProperty { get => "RoleInfo"; }
        public string RoleInfo { get; set; }
        public static string DescriptionProperty { get => "Description"; }
        public string Description { get; set; }
        public static string EmailAddressProperty { get => "EmailAddress"; }
        public string EmailAddress { get; set; }
        public static string PhoneNumberProperty { get => "PhoneNumber"; }
        public string PhoneNumber { get; set; }
        public static string LinkedInProfileLinkProperty { get => "LinkedInProfileLink"; }
        public string LinkedInProfileLink { get; set; }
        public static string FacebookProfileLinkProperty { get => "FacebookProfileLink"; }
        public string FacebookProfileLink { get; set; }
        public static string TwitterScreenNameProperty { get => "TwitterScreenName"; }
        public string TwitterScreenName { get; set; }
        public static string XingProfileLinkProperty { get => "XingProfileLink"; }
        public string XingProfileLink { get; set; }
        public static string SkypeProfileLinkProperty { get => "SkypeProfileLink"; }
        public string SkypeProfileLink { get; set; }
        public static string VimeoProfileLinkProperty { get => "VimeoProfileLink"; }
        public string VimeoProfileLink { get; set; }
        public static string YouTubeProfileLinkProperty { get => "YouTubeProfileLink"; }
        public string YouTubeProfileLink { get; set; }
        public static string StumbleUponProfileLinkProperty { get => "StumbleUponProfileLink"; }
        public string StumbleUponProfileLink { get; set; }
        public static string PinterestProfileLinkProperty { get => "PinterestProfileLink"; }
        public string PinterestProfileLink { get; set; }
        public static string InstagramProfileLinkProperty { get => "InstagramProfileLink"; }
        public string InstagramProfileLink { get; set; }
        public static string PersonalLinksProperty { get => "PersonalLinks"; }
        public IEnumerable<AuthorLink> PersonalLinks { get; set; }
        public static string LocationDetailProperty { get => "LocationDetail"; }
        public string LocationDetail { get; set; }
        public static string LocationCountryProperty { get => "LocationCountry"; }
        public string LocationCountry { get; set; }
        public static string TagLineCompanyProperty { get => "TagLineCompany"; }
        public string TagLineCompany { get; set; }
        public static string SubscribeLinkProperty { get => "SubscribeLink"; }
        public string SubscribeLink { get; set; }
    }
}
