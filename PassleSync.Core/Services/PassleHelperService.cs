using PassleSync.Core.API.Services;
using PassleSync.Core.Helpers.Queries;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Services
{
    public class PassleHelperService : IPassleHelperService
    {
        public PasslePostQuery GetPosts()
        {
            var query = Current.Factory.GetInstance<PasslePostQuery>();
            return query;
        }

        public PassleAuthorQuery GetAuthors()
        {
            var query = Current.Factory.GetInstance<PassleAuthorQuery>();
            return query;
        }
    }
}
