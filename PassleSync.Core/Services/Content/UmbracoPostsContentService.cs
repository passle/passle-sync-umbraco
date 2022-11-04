using Examine;
using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using System.Linq;
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
        public override void ClearFeaturedContent()
        {
            var existingFeaturedContent = GetPublishedContent()
                .Where(x => x.GetValueOrDefault<bool>("IsFeaturedOnPasslePage") || x.GetValueOrDefault<bool>("IsFeaturedOnPostPage"));

            foreach (var contentItem in existingFeaturedContent)
            {
                var editableContentItem = _contentService.GetById(contentItem.Id);

                editableContentItem.SetValue("IsFeaturedOnPasslePage", false);
                editableContentItem.SetValue("IsFeaturedOnPostPage", false);

                _contentService.SaveAndPublish(editableContentItem, raiseEvents: false);
            }
        }
        public override void SetFeaturedContent(string shortcode, bool isFeaturedOnPasslePage, bool isFeaturedOnPostPage)
        {
            var newFeaturedPost = GetContentByShortcode(shortcode);
            if (newFeaturedPost == null)
            {
                return;
            }

            // Set the new post as featured on the Passle page/post page
            var editableContent = _contentService.GetById(newFeaturedPost.Id);

            editableContent.SetValue("IsFeaturedOnPasslePage", isFeaturedOnPasslePage);
            editableContent.SetValue("IsFeaturedOnPostPage", isFeaturedOnPostPage);

            _contentService.SaveAndPublish(editableContent, raiseEvents: false);
        }
    }
}
