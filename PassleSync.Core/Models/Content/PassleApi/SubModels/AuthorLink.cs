using PassleSync.Core.Attributes;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [NestedContentNameTemplate("{{title}}")]
    public class AuthorLink
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
