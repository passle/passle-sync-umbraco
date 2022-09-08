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

        public IEnumerable<IContent> GetPosts()
        {
            return GetContent(PassleContentType.PASSLE_POST);
        }

        public IEnumerable<IContent> GetAuthors()
        {
            return GetContent(PassleContentType.PASSLE_AUTHOR);
        }

        public IContent GetPostByShortcode(string shortcode)
        {
            var posts = GetContent(PassleContentType.PASSLE_POST);
            return posts.Where(x => x.GetValueOrDefault<string>("PostShortcode") == shortcode).FirstOrDefault();
        }

        public IContent GetAuthorByShortcode(string shortcode)
        {
            var authors = GetContent(PassleContentType.PASSLE_AUTHOR);
            return authors.Where(x => x.GetValueOrDefault<string>("Shortcode") == shortcode).FirstOrDefault();
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

        private IEnumerable<IContent> GetContent(string contentType)
        {
            if (!ExamineManager.Instance.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                throw new InvalidOperationException($"No index found with name {UmbracoConstants.UmbracoIndexes.ExternalIndexName}");
            }

            var ids = index.GetSearcher().CreateQuery("content").NodeTypeAlias(contentType).Execute().Select(x => int.Parse(x.Id));
            return _contentService.GetByIds(ids);
        }
    }
}
