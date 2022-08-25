using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PassleAuthors : PaginatedResponseBase
    {
        public IEnumerable<PassleAuthor> People { get; set; }
    }
}
