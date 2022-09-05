using Examine;
using PassleSync.Core.Constants;
using PassleSync.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Examine;
using Umbraco.Web;
using UmbracoConstants = Umbraco.Core.Constants;

namespace PassleSync.Core.Services.Content
{
    public class UmbracoContentService
    {
        public IExamineManager _examineManager;
        protected readonly IContentService _contentService;
        protected readonly UmbracoHelper _umbracoHelper;
        protected readonly ConfigService _configService;
        protected readonly ILogger _logger;

        //https://our.umbraco.com/documentation/implementation/Services/

        public UmbracoContentService(
            IExamineManager examineManager,
            IContentService contentService,
            UmbracoHelper umbracoHelper,
            ConfigService configService,
            ILogger logger)
        {
            _examineManager = examineManager;
            _contentService = contentService;
            _umbracoHelper = umbracoHelper;
            _configService = configService;
            _logger = logger;
        }

        public IEnumerable<IPublishedContent> GetPublishedPosts()
        {
            return GetPublishedContent(PassleContentType.PASSLE_POST);
        }

        public IEnumerable<IPublishedContent> GetPublishedAuthors()
        {
            return GetPublishedContent(PassleContentType.PASSLE_AUTHOR);
        }

        public IPublishedContent GetPublishedPostByShortcode(string shortcode)
        {
            var virtualContent = GetPublishedContent(PassleContentType.PASSLE_POST);
            var matchingContent = virtualContent.Where(x => x.GetValueOrDefault<string>("PostShortcode") == shortcode).FirstOrDefault();
            return matchingContent;
        }

        public IPublishedContent GetPublishedAuthorByShortcode(string shortcode)
        {
            var virtualContent = GetPublishedContent(PassleContentType.PASSLE_AUTHOR);
            var matchingContent = virtualContent.Where(x => x.GetValueOrDefault<string>("Shortcode") == shortcode).FirstOrDefault();
            return matchingContent;
        }

        public IEnumerable<IPublishedContent> GetPublishedContent(string contentType)
        {
            if (_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                var ids = index.GetSearcher().CreateQuery("content").NodeTypeAlias(contentType).Execute().Select(x => x.Id);

                foreach (var id in ids)
                {
                    yield return _umbracoHelper.Content(id);
                }
            }
        }

        public void Delete(IContent document)
        {
            if (document != null)
            {
                try
                {
                    _contentService.Delete(document);
                }
                catch (Exception ex)
                {
                    _logger.Error(_contentService.GetType(), ex, $"Failed to delete umbraco content: {ex.Message}");
                    return;
                }
            }
        }
    }
}
