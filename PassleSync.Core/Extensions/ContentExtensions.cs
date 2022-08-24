using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace PassleSync.Core.Extensions
{
    public static class ContentExtensions
    {
        public static void SetValueIfChanged(this IContent content, string propertyTypeAlias, object value, string culture = null, string segment = null)
        {
            var currentValue = content.GetValue(propertyTypeAlias);
            if (Equals(currentValue, value))
            {
                return;
            }

            content.SetValue(propertyTypeAlias, value, culture, segment);
        }

        //public static T GetValueOrDefault<T>(this IPublishedContent content, string key, T defaultValue = default(T))
        //{
        //    try
        //    {
        //        return (T)content?.GetType()?.GetProperty(key)?.GetValue(content, null) ?? defaultValue;
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return defaultValue;
        //    }
        //}

        public static T GetValueOrDefault<T>(this IContent content, string key, T defaultValue = default(T))
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
    }
}
