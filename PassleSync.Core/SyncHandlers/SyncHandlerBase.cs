using Newtonsoft.Json;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Extensions;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace PassleSync.Core.SyncHandlers
{
    public abstract class SyncHandlerBase<T> : ISyncHandler<T>
    {
        protected readonly IContentService _contentService;
        protected readonly ConfigService _configService;
        protected readonly PassleContentService _passleContentService;
        protected readonly ILogger _logger;


        public SyncHandlerBase(
            IContentService contentService,
            ConfigService configService,
            PassleContentService passleContentService,
            ILogger logger)
        {
            _contentService = contentService;
            _configService = configService;
            _passleContentService = passleContentService;
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

        protected void AddAllPropertiesToNode(IContent node, T entity)
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyTypeInfo = property.PropertyType;
                var isEnumerable = false;

                if (propertyTypeInfo.Implements<IEnumerable>() && propertyTypeInfo.IsGenericType)
                {
                    propertyTypeInfo = propertyTypeInfo.GetGenericArguments()[0];
                    isEnumerable = true;
                }
                else if (!propertyTypeInfo.IsSimpleType())
                {
                    continue;
                }

                if (propertyTypeInfo.IsSerializable)
                {
                    if (isEnumerable)
                    {
                        AddRepeatableTextstringsToNode(node, entity, property.Name);
                    }
                    else if (propertyTypeInfo.IsEnum)
                    {
                        AddEnumToNode(node, entity, property.Name);
                    }
                    else
                    {
                        AddPropertyToNode(node, entity, property.Name);
                    }
                }
                else
                {
                    AddNestedContentToNode(node, entity, propertyTypeInfo, property.Name);
                }
            }
        }

        private void AddPropertyToNode(IContent node, T entity, string propertyName)
        {
            var value = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            node.SetValue(propertyName.ToPropertyAlias(), value);
        }

        private void AddRepeatableTextstringsToNode(IContent node, T entity, string propertyName)
        {
            var items = (IEnumerable<string>) entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            if (items == null)
            {
                return;
            }

            var value = string.Join(Environment.NewLine, items);

            node.SetValue(propertyName.ToPropertyAlias(), value);
        }

        private void AddEnumToNode(IContent node, T entity, string propertyName)
        {
            var value = (Enum) entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            var enumDescription = value.GetDescription();

            node.SetValue(propertyName.ToPropertyAlias(), enumDescription);
        }

        private void AddNestedContentToNode(IContent node, T entity, Type type, string propertyName)
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
                    dictionary.Add(itemProperty.Name.ToPropertyAlias(), value);
                }

                result.Add(dictionary);
            }

            node.SetValue(propertyName, JsonConvert.SerializeObject(result));
        }
    }
}
