using Examine;
using PassleSync.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.Services.Content
{
    public abstract class UmbracoContentService<T>
    {
        public IExamineManager _examineManager;
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly ILogger _logger;

        protected int _parentNodeId;
        protected string _contentTypeAlias;

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

        public abstract string Name(T item);
        public abstract string Shortcode(IContent item);

        public IEnumerable<IContent> GetExistingContent()
        {
            if (_contentService.HasChildren(_parentNodeId))
            {
                return _contentService.GetPagedChildren(_parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
        }

        public void Create(T item)
        {
            var node = _contentService.Create(Name(item), _parentNodeId, _contentTypeAlias);

            node.AddAllPropertiesToNode(item);

            _contentService.SaveAndPublish(node);
        }


        public void DeleteAll()
        {
            // Delete all existing items
            foreach (var child in GetExistingContent())
            {
                Delete(child);
            }
        }

        public void DeleteMany(string[] shortcodes)
        {
            // Delete any existing items with matching shortcodes
            foreach (var child in GetExistingContent())
            {
                if (shortcodes.Contains(Shortcode(child)))
                {
                    Delete(child);
                }
            }
        }

        public void DeleteOne(string shortcode)
        {
            // Delete any existing items with matching shortcodes
            foreach (var child in GetExistingContent())
            {
                if (shortcode == Shortcode(child))
                {
                    Delete(child);
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
