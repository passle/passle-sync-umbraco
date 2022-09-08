using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    public class PassleAuthorQuery : QueryBase<PassleAuthor>
    {
        protected override string ContentType => PassleContentType.PASSLE_AUTHOR;
        protected override string[] SearchFields => new string[] { "nodeName", "roleInfo", "description" };

        public PassleAuthorQuery(IExamineManager examineManager, UmbracoHelper umbracoHelper) : base(examineManager, umbracoHelper)
        {
        }

        public PassleAuthorQuery ByShortcodes(IEnumerable<string> shortcodes)
        {
            _query = _query.And().GroupedOr(new string[] { "shortcode" }, shortcodes.ToArray());
            return this;
        }
    }
}
