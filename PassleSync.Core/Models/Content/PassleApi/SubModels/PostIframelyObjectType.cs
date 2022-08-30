using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PostIframelyObjectType
    {
        None = 0,
        Photo = 1,
        Video = 2,
        Link = 3,
        Rich = 4,
    }
}
