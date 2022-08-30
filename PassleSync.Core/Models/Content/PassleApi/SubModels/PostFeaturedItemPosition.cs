using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PostFeaturedItemPosition
    {
        None = 0,
        ContentBottom = 1,
        ContentTop = 2,
        Header = 3
    }
}
