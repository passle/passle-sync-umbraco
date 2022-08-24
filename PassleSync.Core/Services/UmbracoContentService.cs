using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.Services.Content
{
    public class UmbracoContentService
    {
        public IExamineManager _examineManager;
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly ILogger _logger;

        private readonly string _postDocumentType = "passlePost";
        private readonly string _authorDocumentType = "passleAuthor";
        public string PostDocumentType { get => _postDocumentType; }
        public string AuthorDocumentType { get => _authorDocumentType; }

        //https://our.umbraco.com/documentation/implementation/Services/

        public UmbracoContentService(
            IExamineManager examineManager,
            IContentService contentService,
            ConfigService configService,
            ILogger logger)
        {
            _examineManager = examineManager;
            _contentService = contentService;
            _configService = configService;
            _logger = logger;
        }

        public IEnumerable<IContent> GetExistingPosts()
        {
            return GetExistingContent(_configService.PostsParentNodeId);
        }

        public IEnumerable<IContent> GetExistingAuthors()
        {
            return GetExistingContent(_configService.AuthorsParentNodeId);
        }

        public IEnumerable<IContent> GetExistingContent(int parentNodeId)
        {
            if (_contentService.HasChildren(parentNodeId))
            {
                return _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
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
