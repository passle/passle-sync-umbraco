using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PostMediaType
    {
        None = 0,
        Image = 1,
        Video = 2,
        Audio = 3,
        Embed = 4,
        Font = 5,
        Document = 6
    }
}
