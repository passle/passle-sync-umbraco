using Examine;
using PassleSync.Core.Extensions;
using PassleSync.Core.SyncHandlers;
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
                .Execute(int.MaxValue)
                .Select(x => int.Parse(x.Id));
        }

        public IPublishedContent GetContentByShortcode(string shortcode)
        {
            return GetPublishedContent().Where(x => Shortcode(x) == shortcode).FirstOrDefault();
        }

        public SyncTaskResult Create(T item)
        {
            var node = _contentService.Create(Name(item), _parentNodeId, _contentTypeAlias);

            OnBeforeSave(node, item);

            node.AddAllPropertiesToNode(item);

            var publishResult = _contentService.SaveAndPublish(node);

            var result = new SyncTaskResult()
            {
                Shortcode = Shortcode(node),
                Content = node,
                Success = publishResult.Success
            };

            return result;
        }

        public SyncTaskResult UpdateOne(IPublishedContent publishedContent, T item)
        {
            var editableContent = _contentService.GetById(publishedContent.Id);

            OnBeforeSave(editableContent, item);

            editableContent.AddAllPropertiesToNode(item);

            var publishResult = _contentService.SaveAndPublish(editableContent, raiseEvents: false);

            var result = new SyncTaskResult()
            {
                Shortcode = Shortcode(editableContent),
                Content = editableContent,
                Success = publishResult.Success
            };

            return result;
        }


        public IEnumerable<SyncTaskResult> DeleteAll()
        {
            var results = new List<SyncTaskResult>();

            // Delete all existing items
            var children = GetAllContent();
            foreach (var child in children)
            {
                results.Add(new SyncTaskResult()
                {
                    Shortcode = Shortcode(child),
                    Content = child,
                    Success = Delete(child)
                });
            }

            return results;
        }

        public IEnumerable<SyncTaskResult> DeleteMany(string[] shortcodes)
        {
            var results = new List<SyncTaskResult>();

            // Delete any existing items with matching shortcodes
            var children = GetAllContent().Where(x => shortcodes.Contains(Shortcode(x)));
            foreach (var child in children)
            {
                results.Add(new SyncTaskResult()
                {
                    Shortcode = Shortcode(child),
                    Content = child,
                    Success = Delete(child)
                });
            }

            return results;
        }

        public SyncTaskResult DeleteOne(string shortcode)
        {
            var result = new SyncTaskResult() {
                Shortcode = shortcode,
                Success = true
            };

            // Delete any existing items with matching shortcodes
            var children = GetAllContent().Where(x => shortcode == Shortcode(x));
            foreach (var child in children)
            {
                result.Content = child;

                var success = Delete(child);
                result.Success &= success;
            }

            return result;
        }

        public bool Delete(IContent document)
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
                    return result.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(_contentService.GetType(), ex, $"Failed to delete umbraco content: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public virtual void ClearFeaturedContent()
        { }
        public virtual void SetFeaturedContent(string shortcode, bool isFeaturedOnPasslePage, bool isFeaturedOnPostPage)
        { }
    }
}
