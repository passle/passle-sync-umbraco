using System.ComponentModel;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public enum PostFeaturedItemPosition
    {
        None = 0,
        [Description("Bottom")]
        ContentBottom = 1,
        [Description("Top")]
        ContentTop = 2,
        [Description("Header")]
        Header = 3
    }
}
