using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Core.Models;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardAuthorViewModel
    {
        public int Id;
        public string Name;
        public string Shortcode;
        public string RoleInfo;
        public string Description;
        public string ProfileUrl;
        public string AvatarUrl;
        public bool Synced;

        public PassleDashboardAuthorViewModel(IContent from)
        {
            // TODO: Can we use reflection or constants to make this more robust than magic strings?

            Id = from?.Id ?? 0;
            Name = from.GetValueOrDefault<string>("passleName");
            Shortcode = from.GetValueOrDefault<string>("shortcode");
            RoleInfo = from.GetValueOrDefault<string>("roleInfo");
            Description = from.GetValueOrDefault<string>("description");
            ProfileUrl = from.GetValueOrDefault<string>("profileUrl");
            AvatarUrl = from.GetValueOrDefault<string>("avatarUrl");
            Synced = true;
        }

        public PassleDashboardAuthorViewModel(PassleAuthor from)
        {
            Id = -1;
            Name = from.Name;
            Shortcode = from.Shortcode;
            RoleInfo = from.RoleInfo;
            Description = from.Description;
            ProfileUrl = from.ProfileUrl;
            AvatarUrl = from.AvatarUrl;
            Synced = false;
        }
    }
}
