using System.Collections.Specialized;

namespace PassleSync.Core.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string GetValueOrDefault(this NameValueCollection collection, string key, string defaultValue = default)
        {
            return collection[key] ?? defaultValue;
        }
    }
}
