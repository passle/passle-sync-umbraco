﻿using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using Umbraco.Web;

namespace PassleSync.Core.Helpers.Queries
{
    public class PassleAuthorQuery : QueryBase<PassleAuthor>
    {
        protected override string ContentType => PassleContentType.PASSLE_AUTHOR;

        public PassleAuthorQuery(IExamineManager examineManager, UmbracoHelper umbracoHelper) : base(examineManager, umbracoHelper)
        {
        }
    }
}
