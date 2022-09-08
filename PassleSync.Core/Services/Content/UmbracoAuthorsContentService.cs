﻿using Examine;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.Services.Content
{
    public class UmbracoAuthorsContentService : UmbracoContentService<PassleAuthor>
    {
        public UmbracoAuthorsContentService(
            IExamineManager examineManager,
            IContentService contentService,
            ConfigService configService,
            ILogger logger
        ) : base (
            examineManager,
            contentService,
            configService,
            logger
        )
        {
            _parentNodeId = configService.AuthorsParentNodeId;
            _contentTypeAlias = _configService.PassleAuthorContentTypeAlias;
        }

        public override string Name(PassleAuthor item)
        {
            return item.Name;
        }

        public override string Shortcode(IContent item)
        {
            return item.GetValue<string>("shortcode");
        }

        public override void OnBeforeSave(IContent node, PassleAuthor item)
        {
            node.Name = item.Name;
        }
    }
}