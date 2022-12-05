using Newtonsoft.Json;
using PassleSync.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors.ValueConverters;
using Umbraco.Web.PublishedCache;

namespace PassleSync.Core.PropertyValueConverters
{
    public class PassleTagsPropertyValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.InvariantEquals(PassleDataType.TAGS);
        }
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(IEnumerable<string>);

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return "";
            return source.ToString();
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.None;
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null) return Enumerable.Empty<string>();

            var interString = inter.ToString();
            if (string.IsNullOrEmpty(interString)) return Enumerable.Empty<string>();

            return JsonConvert.DeserializeObject<IEnumerable<string>>(inter.ToString());
        }
    }
}
