using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    /// <summary>
    /// A query for models of type <see cref="PassleAuthor"/>.
    /// </summary>
    public class PassleAuthorQuery : QueryBase<PassleAuthorQuery, PassleAuthor>
    {
        protected override string ContentType => PassleContentType.PASSLE_AUTHOR;
        protected override string[] SearchFields => new string[] { "nodeName", "roleInfo", "description" };

        public PassleAuthorQuery(IExamineManager examineManager, UmbracoHelper umbracoHelper) : base(examineManager, umbracoHelper)
        {
        }

        /// <summary>
        /// Filter to authors that match one of the shortcodes specified.
        /// </summary>
        public PassleAuthorQuery ByShortcodes(IEnumerable<string> shortcodes)
        {
            _query = _query.And().GroupedOr(new string[] { "shortcode" }, shortcodes.ToArray());
            return this;
        }
    }
}
