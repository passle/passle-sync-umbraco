using Examine;
using PassleSync.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Examine;
using UmbracoConstants = Umbraco.Core.Constants;

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
        public virtual void OnBeforeSave(IContent node, T item)
        { }

        public IEnumerable<IContent> GetContent()
        {
            if (_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.InternalIndexName, out var index))
            {
                var ids = index.GetSearcher().CreateQuery("content").NodeTypeAlias(_contentTypeAlias).Execute().Select(x => int.Parse(x.Id));
                return _contentService.GetByIds(ids);
            }

            return null;
        }

        public IContent GetContentByShortcode(string shortcode)
        {
            return GetContent().Where(x => Shortcode(x) == shortcode).FirstOrDefault();
        }

        public void Create(T item)
        {
            var node = _contentService.Create(Name(item), _parentNodeId, _contentTypeAlias);

            OnBeforeSave(node, item);

            node.AddAllPropertiesToNode(item);

            _contentService.SaveAndPublish(node);
        }

        public void UpdateOne(IContent publishedContent, T item)
        {
            var editableContent = _contentService.GetById(publishedContent.Id);

            OnBeforeSave(editableContent, item);

            editableContent.AddAllPropertiesToNode(item);

            _contentService.SaveAndPublish(editableContent, raiseEvents: false);
        }


        public void DeleteAll()
        {
            // Delete all existing items
            foreach (var child in GetContent())
            {
                Delete(child);
            }
        }

        public void DeleteMany(string[] shortcodes)
        {
            // Delete any existing items with matching shortcodes
            foreach (var child in GetContent())
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
            foreach (var child in GetContent())
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
