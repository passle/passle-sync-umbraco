using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PasslePosts : PaginatedResponseBase
    {
        public IEnumerable<PasslePost> Posts { get; set; }
    }
}
