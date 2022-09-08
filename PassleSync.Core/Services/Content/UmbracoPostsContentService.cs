﻿using Examine;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.Services.Content
{
    public class UmbracoPostsContentService : UmbracoContentService<PasslePost>
    {
        public UmbracoPostsContentService(
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
            _parentNodeId = configService.PostsParentNodeId;
            _contentTypeAlias = _configService.PasslePostContentTypeAlias;
        }

        public override string Name(PasslePost item)
        {
            return item.PostTitle;
        }

        public override string Shortcode(IContent item)
        {
            return item.GetValue<string>("postShortcode");
        }

        public override void OnBeforeSave(IContent node, PasslePost item)
        {
            var date = DateTime.Parse(item.PublishedDate);
            node.CreateDate = date;
            node.PublishDate = date;
            node.UpdateDate = date;
        }
    }
}