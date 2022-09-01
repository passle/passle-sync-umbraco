using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

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

        public static T GetValueOrDefault<T>(this IPublishedContent content, string key, T defaultValue = default)
        {
            try
            {
                return (T)content.GetProperty(key).GetValue();
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
        }
    }
}
