using PassleSync.Core.API.Services;
using PassleSync.Core.Helpers.Queries;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Services
{
    public class PassleHelperService : IPassleHelperService
    {
        /// <summary>
        /// Creates a new <see cref="PasslePostQuery"/> instance.
        /// </summary>
        public PasslePostQuery GetPosts()
        {
            var query = Current.Factory.GetInstance<PasslePostQuery>();
            return query;
        }

        /// <summary>
        /// Creates a new <see cref="PassleAuthorQuery"/> instance.
        /// </summary>
        public PassleAuthorQuery GetAuthors()
        {
            var query = Current.Factory.GetInstance<PassleAuthorQuery>();
            return query;
        }
    }
}
