using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    /// <summary>
    /// A query for models of type <see cref="PasslePost"/>.
    /// </summary>
    public class PasslePostQuery : QueryBase<PasslePostQuery, PasslePost>
    {
        protected override string ContentType => PassleContentType.PASSLE_POST;
        protected override string[] SearchFields => new string[] { "nodeName", "postContentHtml", "quoteText" };

        public PasslePostQuery(IExamineManager examineManager, UmbracoHelper umbracoHelper) : base(examineManager, umbracoHelper)
        {
        }

        /// <summary>
        /// Filter based on whether the post should be featured on the Passle page.
        /// </summary>
        public PasslePostQuery FeaturedOnPasslePage(bool includeFeatured)
        {
            _query = _query.And().Field("isFeaturedOnPasslePage", Convert.ToInt32(includeFeatured).ToString());
            return this;
        }

        /// <summary>
        /// Filter based on whether the post should be featured on the post page.
        /// </summary>
        public PasslePostQuery FeaturedOnPostPage(bool includeFeatured)
        {
            _query = _query.And().Field("isFeaturedOnPostPage", Convert.ToInt32(includeFeatured).ToString());
            return this;
        }

        /// <summary>
        /// Filter posts by the the shortcode of a particular author.
        /// </summary>
        public PasslePostQuery ByAuthorShortcode(string shortcode)
        {
            _query = _query.And().Field("authors_shortcode", shortcode);
            return this;
        }

        /// <summary>
        /// Filter to posts that include a particular tag.
        /// </summary>
        public PasslePostQuery ByTag(string tag)
        {
            _query = _query.And().Field("tags", tag);
            return this;
        }

        /// <summary>
        /// Filter to posts that from a particular Passle.
        /// </summary>
        public PasslePostQuery ByPassle(string shortcode)
        {
            _query = _query.And().Field("passleShortcode", shortcode);
            return this;
        }
    }
}
