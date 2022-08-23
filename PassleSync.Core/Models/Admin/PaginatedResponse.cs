using PassleSync.Core.Models.PassleSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassleSync.Core.Models.Admin
{
    public class PaginatedResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Person> People { get; set; }
    }
}
