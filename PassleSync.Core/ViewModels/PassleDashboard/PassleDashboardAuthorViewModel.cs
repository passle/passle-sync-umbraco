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
            Name = from.GetValueOrDefault<string>(PassleAuthor.NameProperty);
            Shortcode = from.GetValueOrDefault<string>(PassleAuthor.ShortcodeProperty);
            RoleInfo = from.GetValueOrDefault<string>(PassleAuthor.RoleInfoProperty);
            Description = from.GetValueOrDefault<string>(PassleAuthor.DescriptionProperty);
            ProfileUrl = from.GetValueOrDefault<string>(PassleAuthor.ProfileUrlProperty);
            AvatarUrl = from.GetValueOrDefault<string>(PassleAuthor.AvatarUrlProperty);
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
