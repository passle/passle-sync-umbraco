using Examine;
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
    public abstract class UmbracoContentService<T>
    {
        public IExamineManager _examineManager;
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly ILogger _logger;
        protected readonly IPublishedContentQuery _publishedContentQuery;

        protected int _parentNodeId;
        protected string _contentTypeAlias;

        //https://our.umbraco.com/documentation/implementation/Services/

        public UmbracoContentService(
            IExamineManager examineManager,
            IContentService contentService,
            ConfigService configService,
            ILogger logger,
            IPublishedContentQuery publishedContentQuery)
        {
            _examineManager = examineManager;
            _contentService = contentService;
            _configService = configService;
            _logger = logger;
            _publishedContentQuery = publishedContentQuery;
        }

        public abstract string Name(T item);
        public abstract string Shortcode(IContent item);
        public abstract string Shortcode(IPublishedContent item);
        public virtual void OnBeforeSave(IContent node, T item)
        { }

        public IEnumerable<IPublishedContent> GetPublishedContent()
        {
            var ids = GetContent(UmbracoConstants.UmbracoIndexes.ExternalIndexName);

            // Call ToList to formalise the result now and avoid errors about 'expired snapshot' when enumerating later
            return _publishedContentQuery.Content(ids).ToList();
        }

        public IEnumerable<IContent> GetAllContent()
        {
            var ids = GetContent(UmbracoConstants.UmbracoIndexes.InternalIndexName);

            return _contentService.GetByIds(ids);
        }

        IEnumerable<int> GetContent(string indexName)
        {
            if (!_examineManager.TryGetIndex(indexName, out var index))
            {
                throw new InvalidOperationException($"No index found with name {indexName}");
            }

            return index.GetSearcher()
                .CreateQuery("content")
                .NodeTypeAlias(_contentTypeAlias)
                .Execute(5000)
                .Select(x => int.Parse(x.Id));
        }

        public IPublishedContent GetContentByShortcode(string shortcode)
        {
            return GetPublishedContent().Where(x => Shortcode(x) == shortcode).FirstOrDefault();
        }

        public void Create(T item)
        {
            var node = _contentService.Create(Name(item), _parentNodeId, _contentTypeAlias);

            OnBeforeSave(node, item);

            node.AddAllPropertiesToNode(item);

            _contentService.SaveAndPublish(node);
        }

        public void UpdateOne(IPublishedContent publishedContent, T item)
        {
            var editableContent = _contentService.GetById(publishedContent.Id);

            OnBeforeSave(editableContent, item);

            editableContent.AddAllPropertiesToNode(item);

            _contentService.SaveAndPublish(editableContent, raiseEvents: false);
        }


        public void DeleteAll()
        {
            // Delete all existing items
            var children = GetAllContent();
            foreach (var child in children)
            {
                Delete(child);
            }
        }

        public void DeleteMany(string[] shortcodes)
        {
            // Delete any existing items with matching shortcodes
            var children = GetAllContent().Where(x => shortcodes.Contains(Shortcode(x)));
            foreach (var child in children)
            {
                Delete(child);
            }
        }

        public void DeleteOne(string shortcode)
        {
            // Delete any existing items with matching shortcodes
            var children = GetAllContent().Where(x => shortcode == Shortcode(x));
            foreach (var child in children)
            {
                Delete(child);
            }
        }

        public void Delete(IContent document)
        {
            if (document != null)
            { 
                try
                {
                   var result =  _contentService.Delete(document);
                    if (!result.Success)
                    {
                        _logger.Debug(_contentService.GetType(), $"Failed to delete umbraco content: {string.Join(", ", result.EventMessages.GetAll().Select(x => x.Message))}");
                    }
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
