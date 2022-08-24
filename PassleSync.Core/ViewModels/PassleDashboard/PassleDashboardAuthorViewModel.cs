using PassleSync.Core.Extensions;
using PassleSync.Core.Models;
using PassleSync.Core.Models.Admin;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

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
            Name = from.GetValueOrDefault<string>("AuthorName");
            Shortcode = from.GetValueOrDefault<string>("Shortcode");
            PassleShortcode = from.GetValueOrDefault<string>("PassleShortcode");
            //ProfileUrl = from?.Url() ?? "";
            AvatarUrl = from.GetValueOrDefault<string>("AvatarUrl");
            Role = from.GetValueOrDefault<string>("Role");
            Description = from.GetValueOrDefault<string>("Description");
            Id = from?.Id ?? 0;
            Synced = true;
        }

        //public PassleDashboardAuthorViewModel(IPublishedContent from)
        //{
        //    Name = from.GetValueOrDefault<string>("AuthorName");
        //    Shortcode = from.GetValueOrDefault<string>("Shortcode");
        //    PassleShortcode = from.GetValueOrDefault<string>("PassleShortcode");
        //    //ProfileUrl = from?.Url() ?? "";
        //    AvatarUrl = from.GetValueOrDefault<string>("AvatarUrl");
        //    Role = from.GetValueOrDefault<string>("Role");
        //    Description = from.GetValueOrDefault<string>("Description");
        //    Id = from?.Id ?? 0;
        //    Synced = true;
        //}

        //public PassleDashboardAuthorViewModel(Person from)
        //{
        //    Name = from.Name;
        //    Shortcode = from.Shortcode;
        //    //PassleShortcode = from.PassleShortcode;
        //    ProfileUrl = from.ProfileUrl;
        //    AvatarUrl = from.AvatarUrl;
        //    Role = from.RoleInfo;
        //    Description = from.Description;
        //    Synced = false;
        //}

        public PassleDashboardAuthorViewModel(PassleAuthor from)
        {
            Name = from.Name;
            Shortcode = from.Shortcode;
            PassleShortcode = from.PassleShortcode;
            ProfileUrl = from.ProfileUrl;
            AvatarUrl = from.AvatarUrl;
            Role = from.RoleInfo;
            Description = from.Description;
            Synced = false;
        }
    }
}
