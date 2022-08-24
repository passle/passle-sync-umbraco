using PassleSync.Core.Extensions;
using PassleSync.Core.Models;
using Umbraco.Core.Models;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardAuthorViewModel
    {
        public string Name;
        public string Shortcode;
        public string PassleShortcode;
        public string ProfileUrl;
        public string AvatarUrl;
        public string Role;
        public string Description;
        public int Id;
        public bool Synced;

        public PassleDashboardAuthorViewModel(IContent from)
        {
            Name = from.GetValueOrDefault<string>("Name");
            Shortcode = from.GetValueOrDefault<string>("Shortcode");
            PassleShortcode = from.GetValueOrDefault<string>("PassleShortcode");
            ProfileUrl = from.GetValueOrDefault<string>("ProfileUrl");
            AvatarUrl = from.GetValueOrDefault<string>("AvatarUrl");
            Role = from.GetValueOrDefault<string>("Role");
            Description = from.GetValueOrDefault<string>("Description");
            Id = from?.Id ?? 0;
            Synced = true;
        }

        public PassleDashboardAuthorViewModel(PassleAuthor from)
        {
            Name = from.Name;
            Shortcode = from.Shortcode;
            //PassleShortcode = from.PassleShortcode;
            ProfileUrl = from.ProfileUrl;
            AvatarUrl = from.AvatarUrl;
            Role = from.RoleInfo;
            Description = from.Description;
            Synced = false;
        }
    }
}
