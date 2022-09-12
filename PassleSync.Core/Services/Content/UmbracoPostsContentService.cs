using Examine;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace PassleSync.Core.Services.Content
{
    public class UmbracoPostsContentService : UmbracoContentService<PasslePost>
    {
        public UmbracoPostsContentService(
            IExamineManager examineManager,
            IContentService contentService,
            ConfigService configService,
            ILogger logger,
            IPublishedContentQuery publishedContentQuery
        ) : base (
            examineManager,
            contentService,
            configService,
            logger,
            publishedContentQuery
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

        public override string Shortcode(IPublishedContent item)
        {
            return item.Value<string>("postShortcode");
        }

        public override void OnBeforeSave(IContent node, PasslePost item)
        {
            node.Name = item.PostTitle;
            node.CreateDate = item.PublishedDate;
            node.PublishDate = item.PublishedDate;
            node.UpdateDate = item.PublishedDate;
        }
    }
}
