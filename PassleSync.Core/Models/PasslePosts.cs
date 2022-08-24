using System.Collections.Generic;

namespace PassleSync.Core.Models
{
    public class PasslePosts : PaginatedResponseBase
    {
        public IEnumerable<PasslePost> Posts { get; set; }
    }
}
