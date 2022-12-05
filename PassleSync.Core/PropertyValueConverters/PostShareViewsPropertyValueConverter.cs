using PassleSync.Core.Models.Content.PassleApi;
using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core;
using PassleSync.Core.Extensions;

namespace PassleSync.Core.PropertyValueConverters
{
    public class PostShareViewsPropertyValueConverter :  IPropertyValueConverter
    {        
        public bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.Alias.ToLower().Equals("shareviews");
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
            return typeof(PostShareViews);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.None;
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return null;

            var attemptConvert = source.TryConvertTo<PostShareViews>();
            if (attemptConvert.Success)
                return attemptConvert.Result;

            return source;
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            return inter;
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null) return null;
            return inter.ToString();
        }
    }
}
