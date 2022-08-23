using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassleDotCom.PasslePlugin.Core.Models.PassleSync
{
    public class PassleAuthor
    {
        public string Name { get; set; }

        public string Shortcode { get; set; }
        public string ProfileUrl { get; set; }
        public string AvatarUrl { get; set; }
        public string SubscribeLink { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        //linkedin_profile_link
        public string LinkedinProfileLink { get; set; }
        //facebook_profile_link
        public string FacebookProfileLink { get; set; }

        //twitter_screen_name
        public string TwitterScreenName { get; set; }

        //xing_profile_link
        public string xing_profile_link { get; set; }

        //skype_profile_link
        public string skype_profile_link { get; set; }

        //vimeo_profile_link
        public string vimeo_profile_link { get; set; }

        public string youtube_profile_link { get; set; }

        public string stumbleupon_profile_link { get; set; }

        public string pinterest_profile_link { get; set; }

        public string instagram_profile_link { get; set; }

        public string personal_links { get; set; }

        public string location_detail { get; set; }

        public string location_country { get; set; }

        public string location_full { get; set; }

        public string company_tagline { get; set; }
    }
}
