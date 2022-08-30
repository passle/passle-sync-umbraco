using Newtonsoft.Json;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using System;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.SyncHandlers
{
    public abstract class SyncHandlerBase<T> : ISyncHandler<T>
    {
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly ILogger _logger;


        public SyncHandlerBase(
            IContentService contentService,
            ConfigService configService,
            ILogger logger)
        {
            _contentService = contentService;
            _configService = configService;
            _logger = logger;
        }

        public abstract IPassleDashboardViewModel GetAll();
        public abstract bool SyncOne(string Shortcode);
        public abstract bool SyncMany(string[] Shortcodes);
        public abstract bool SyncAll();
        public abstract bool DeleteMany(string[] Shortcodes);
        public abstract void DeleteMany(string[] Shortcodes, int parentNodeId);
        public abstract void DeleteAll(int parentNodeId);
        public abstract bool DeleteAll();
        public abstract void CreateOne(T entity, int parentNodeId);
        public abstract void CreateMany(IEnumerable<T> entities, int parentNodeId, string[] shortcodes);
        public abstract void CreateAll(IEnumerable<T> entities, int parentNodeId);

        protected void AddPropertyToNode(IContent node, T entity, string propertyName)
        {
            var value = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            node.SetValue(propertyName, value);
        }

        protected void AddNestedContentToNode(IContent node, T entity, Type type, string propertyName)
        {
            var items = (IEnumerable<object>) entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            if (items == null)
            {
                return;
            }

            var result = new List<Dictionary<string, object>>();

            foreach (var item in items)
            {
                var guid = Guid.NewGuid();
                var contentTypeAlias = type.Name.FirstCharToLower();

                var dictionary = new Dictionary<string, object>()
                {
                    { "key", guid.ToString() },
                    { "name", guid.ToString() }, // TODO: Change this
                    { "ncContentTypeAlias", contentTypeAlias },
                };

                var properties = item.GetType().GetProperties();

                foreach (var itemProperty in properties)
                {
                    var value = item.GetType().GetProperty(itemProperty.Name).GetValue(item, null);
                    dictionary.Add(itemProperty.Name, value);
                }

                result.Add(dictionary);
            }

            node.SetValue(propertyName, JsonConvert.SerializeObject(result));
        }
    }
}
