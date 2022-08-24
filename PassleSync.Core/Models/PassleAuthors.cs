using System.Collections.Generic;

namespace PassleSync.Core.Models
{
    public class PassleAuthors : PaginatedResponseBase
    {
        public IEnumerable<PassleAuthor> People { get; set; }
    }
}
