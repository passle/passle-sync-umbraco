using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PostShareViews
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PostShareButtons SocialNetwork { get; set; }
        public int TotalViews { get; set; }
    }
}
