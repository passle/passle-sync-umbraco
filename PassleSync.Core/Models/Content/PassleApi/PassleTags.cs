using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PassleTags : PaginatedResponseBase
    {
        public IEnumerable<PassleTag> Tags { get; set; }
    }
}
