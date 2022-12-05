using PassleSync.Core.Models.Content.Umbraco;
using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core;
using PassleSync.Core.Extensions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PassleSync.Core.API.Models;
using Umbraco.Web.PublishedCache;
using Umbraco.Web;

namespace PassleSync.Core.PropertyValueConverters
{
    public class PostAuthorInter
    {
        public string key;
        public Guid Key => new Guid(key);
        //public string name;
        //public string ncContentTypeAlias;
        public string shortcode;
        public string passleName;
        public string imageUrl;
        public string profileUrl;
        public string role;
        public string twitterScreenName;
    }

    public class PostAuthorPropertyValueConverter : IPropertyValueConverter
    {
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

        //Injecting the PublishedSnapshotAccessor for fetching content
        public PostAuthorPropertyValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
        }
        public bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.Alias.ToLower().Equals("authors") || propertyType.Alias.ToLower().Equals("coauthors");
        }

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            switch (level)
            {
                case PropertyValueLevel.Source:
                    return value != null && (!(value is string) || string.IsNullOrWhiteSpace((string)value) == false);
                default:
                    throw new NotSupportedException($"Invalid level: {level}.");
            }
        }

        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(IEnumerable<PostAuthor>);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.None;
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return null;
            return ConvertStringToType((string)source);
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null) return null;
            if (inter is string stringInter)
            {
                return ConvertStringToType(stringInter);
            }
            else
            {
                return inter;
            }
        }

        IEnumerable<PostAuthor> ConvertStringToType(string storedData)
        {
            if (storedData.DetectIsJson())
            {
                var postAuthorInters = JsonConvert.DeserializeObject<IEnumerable<PostAuthorInter>>(storedData);

                IEnumerable<IPublishedContent> content = postAuthorInters.Select(x => _publishedSnapshotAccessor.PublishedSnapshot.Content.GetById(x.Key));

                return content.Select(x => (PostAuthor)Activator.CreateInstance(typeof(PostAuthor), new object[] { x }));
            }

            return null;
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null) return null;
            return inter.ToString();
        }
    }
}
