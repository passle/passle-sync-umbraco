using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;

namespace PassleSync.Core.API.Services
{
    public interface IPassleHelperService
    {
        IEnumerable<PasslePost> GetPosts();
        IEnumerable<PassleAuthor> GetAuthors();
    }
}
