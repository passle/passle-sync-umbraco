using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    public class PasslePostQuery : QueryBase<PasslePost>
    {
        protected override string ContentType => PassleContentType.PASSLE_POST;

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
    }
}
