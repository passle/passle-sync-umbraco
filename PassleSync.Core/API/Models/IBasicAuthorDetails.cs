namespace PassleSync.Core.API.Models
{
    public interface IBasicAuthorDetails
    {
        string PassleName { get; }
        string ProfileUrl { get; }
        string RoleInfo { get; }
        string Shortcode { get; }
        string TwitterScreenName { get; }
        string GetAvatarUrl(string fallbackUrl = default);
    }
}
