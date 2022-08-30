using PassleSync.Core.Attributes;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [NestedContentNameTemplate("{{passleName}}")]
    public class PostAuthor
    {
        public string Shortcode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
        public string Role { get; set; }
        public string TwitterScreenName { get; set; }
    }
}
