using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace PassleSync.Core.SyncHandlers
{
    public abstract class SyncHandlerBase<TPlural, TSingular> : ISyncHandler<TSingular>
        where TPlural : PaginatedResponseBase
        where TSingular : class
    {
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly PassleContentService<TPlural, TSingular> _passleContentService;
        protected readonly UmbracoContentService<TSingular> _umbracoContentService;
        protected readonly ILogger _logger;

        public SyncHandlerBase(
            IContentService contentService,
            ConfigService configService,
            PassleContentService<TPlural, TSingular> passleContentService,
            UmbracoContentService<TSingular> umbracoContentService,
            ILogger logger)
        {
            _contentService = contentService;
            _configService = configService;
            _passleContentService = passleContentService;
            _umbracoContentService = umbracoContentService;
            _logger = logger;
        }

        public abstract IPassleDashboardViewModel GetAll();
        public abstract IPassleDashboardViewModel GetExisting();
        public abstract string Shortcode(TSingular item);

        public virtual IEnumerable<SyncTaskResult> SyncAll()
        {
            var apiItems = _passleContentService.GetAll();
            if (apiItems == null)
            {
                throw new Exception("Failed to get items from the API");
            }

            DeleteAll();
            return CreateAll(apiItems);
        }

        public virtual IEnumerable<SyncTaskResult> SyncMany(string[] shortcodes)
        {
            var apiItems = _passleContentService.GetMany(shortcodes);
            if (apiItems == null)
            {
                throw new Exception("Failed to get items from the API");
            }

            var results = new List<SyncTaskResult>();
            foreach (var apiItem in apiItems)
            {
                results.Add(UpdateOrCreateOne(apiItem));
            }
            return results;
        }

        public virtual SyncTaskResult SyncOne(string shortcode)
        {
            var apiItem = _passleContentService.GetOne(shortcode);
            if (apiItem == null)
            {
                throw new Exception("Failed to get item from the API");
            }
            return UpdateOrCreateOne(apiItem);
        }

        public virtual SyncTaskResult UpdateOrCreateOne(TSingular apiItem)
        {
            var publishedContent = _umbracoContentService.GetContentByShortcode(Shortcode(apiItem));
            if (publishedContent == null)
            {
                return CreateOne(apiItem);
            }
            else
            {
                return UpdateOne(publishedContent, apiItem);
            }
        }

        public virtual IEnumerable<SyncTaskResult> DeleteAll()
        {
            return _umbracoContentService.DeleteAll();
        }

        public virtual IEnumerable<SyncTaskResult> DeleteMany(string[] shortcodes)
        {
            return _umbracoContentService.DeleteMany(shortcodes);
        }

        public virtual SyncTaskResult DeleteOne(string shortcode)
        {
            return _umbracoContentService.DeleteOne(shortcode);
        }

        public virtual IEnumerable<SyncTaskResult> CreateAll(IEnumerable<TSingular> items)
        {
            var results = new List<SyncTaskResult>();
            foreach (TSingular item in items)
            {
                results.Add(CreateOne(item));
            }
            return results;
        }

        public virtual IEnumerable<SyncTaskResult> CreateMany(IEnumerable<TSingular> items, string[] shortcodes)
        {
            var results = new List<SyncTaskResult>();
            foreach (TSingular item in items)
            {
                if (shortcodes.Contains(Shortcode(item)))
                {
                    results.Add(CreateOne(item));
                }
            }
            return results;
        }

        public virtual SyncTaskResult CreateOne(TSingular item)
        {
            return _umbracoContentService.Create(item);
        }
        
        public virtual SyncTaskResult UpdateOne(IPublishedContent node, TSingular item)
        {
            return _umbracoContentService.UpdateOne(node, item);
        }

        public virtual void UpdateFeaturedContent(string shortcode, bool isFeaturedOnPasslePage, bool isFeaturedOnPostPage)
        {  }
    }
}
