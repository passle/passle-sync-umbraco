using System;
using Umbraco.Core.Models;

namespace PassleSync.Core.Extensions
{
    public static class ContentExtensions
    {
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
