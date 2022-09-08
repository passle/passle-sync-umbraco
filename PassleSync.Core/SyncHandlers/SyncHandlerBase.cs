using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
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
        public abstract string Shortcode(TSingular item);

        public virtual void SyncAll()
        {
            var apiItems = _passleContentService.GetAll();
            if (apiItems == null)
            {
                throw new Exception("Failed to get items from the API");
            }

            DeleteAll();
            CreateAll(apiItems);
        }

        public virtual void SyncMany(string[] shortcodes)
        {
            var apiItems = _passleContentService.GetAll();
            if (apiItems == null)
            {
                throw new Exception("Failed to get items from the API");
            }

            DeleteMany(shortcodes);
            CreateMany(apiItems, shortcodes);
        }

        public virtual void SyncOne(string shortcode)
        {
            var apiItems = _passleContentService.GetAll();
            if (apiItems == null)
            {
                throw new Exception("Failed to get items from the API");
            }

            var apiItem = apiItems.FirstOrDefault(x => Shortcode(x) == shortcode);
            if (apiItem == null)
            {
                return;
            }

            var publishedContent = _umbracoContentService.GetContentByShortcode(shortcode);
            if (publishedContent == null)
            {
                CreateOne(apiItem);
            }
            else
            {
                UpdateOne(publishedContent, apiItem);
            }
        }

        public virtual void DeleteAll()
        {
            _umbracoContentService.DeleteAll();
        }

        public virtual void DeleteMany(string[] shortcodes)
        {
            _umbracoContentService.DeleteMany(shortcodes);
        }

        public virtual void DeleteOne(string shortcode)
        {
            _umbracoContentService.DeleteOne(shortcode);
        }

        public virtual void CreateAll(IEnumerable<TSingular> items)
        {
            foreach (TSingular item in items)
            {
                CreateOne(item);
            }
        }

        public virtual void CreateMany(IEnumerable<TSingular> items, string[] shortcodes)
        {
            foreach (TSingular item in items)
            {
                if (shortcodes.Contains(Shortcode(item)))
                {
                    CreateOne(item);
                }
            }
        }

        public virtual void CreateOne(TSingular item)
        {
            _umbracoContentService.Create(item);
        }
        
        public virtual void UpdateOne(IContent node, TSingular item)
        {
            _umbracoContentService.UpdateOne(node, item);
        }
    }
}
