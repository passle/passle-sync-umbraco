using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    public class PasslePostQuery : QueryBase<PasslePostQuery, PasslePost>
    {
        protected override string ContentType => PassleContentType.PASSLE_POST;
        protected override string[] SearchFields => new string[] { "nodeName", "postContentHtml", "quoteText" };

        public PasslePostQuery(IExamineManager examineManager, UmbracoHelper umbracoHelper) : base(examineManager, umbracoHelper)
        {
        }

        public PasslePostQuery FeaturedOnPasslePage(bool includeFeatured)
        {
            _query = _query.And().Field("isFeaturedOnPasslePage", Convert.ToInt32(includeFeatured).ToString());
            return this;
        }

        public PasslePostQuery FeaturedOnPostPage(bool includeFeatured)
        {
            _query = _query.And().Field("isFeaturedOnPostPage", Convert.ToInt32(includeFeatured).ToString());
            return this;
        }

        public PasslePostQuery ByAuthorShortcode(string shortcode)
        {
            _query = _query.And().Field("authors_shortcode", shortcode);
            return this;
        }

        public PasslePostQuery ByTag(string tag)
        {
            _query = _query.And().Field("tags", tag);
            return this;
        }
    }
}
