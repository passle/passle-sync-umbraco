using PassleSync.Core.Helpers.Queries;

namespace PassleSync.Core.API.Services
{
    public interface IPassleHelperService
    {
        PasslePostQuery GetPosts();
        PassleAuthorQuery GetAuthors();
    }
}
