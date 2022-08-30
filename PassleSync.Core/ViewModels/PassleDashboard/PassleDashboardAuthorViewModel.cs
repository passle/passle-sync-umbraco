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
            Id = from?.Id ?? 0;
            Name = from.GetValueOrDefault<string>("Name");
            Shortcode = from.GetValueOrDefault<string>("Shortcode");
            RoleInfo = from.GetValueOrDefault<string>("RoleInfo");
            Description = from.GetValueOrDefault<string>("Description");
            ProfileUrl = from.GetValueOrDefault<string>("ProfileUrl");
            AvatarUrl = from.GetValueOrDefault<string>("AvatarUrl");
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
