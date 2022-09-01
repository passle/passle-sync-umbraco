using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace PassleSync.Core.Extensions
{
    public static class ContentExtensions
    {
        public static T GetValueOrDefault<T>(this IContent content, string key, T defaultValue = default)
        {
            try
            {
                return (T)content.GetValue(key);
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
        }

        public static void AddAllPropertiesToNode<T>(this IContent node, T entity)
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

        public static void AddPropertyToNode<T>(this IContent node, T entity, string propertyName)
        {
            var value = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            node.SetValue(propertyName.ToPropertyAlias(), value);
        }

        public static void AddRepeatableTextstringsToNode<T>(this IContent node, T entity, string propertyName)
        {
            var items = (IEnumerable<string>)entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            if (items == null)
            {
                return;
            }

            var value = string.Join(Environment.NewLine, items);

            node.SetValue(propertyName.ToPropertyAlias(), value);
        }

        public static void AddEnumToNode<T>(this IContent node, T entity, string propertyName)
        {
            var value = (Enum)entity.GetType().GetProperty(propertyName).GetValue(entity, null);
            var enumDescription = value.GetDescription();

            node.SetValue(propertyName.ToPropertyAlias(), enumDescription);
        }

        public static void AddNestedContentToNode<T>(this IContent node, T entity, Type type, string propertyName)
        {
            var items = (IEnumerable<object>)entity.GetType().GetProperty(propertyName).GetValue(entity, null);
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
