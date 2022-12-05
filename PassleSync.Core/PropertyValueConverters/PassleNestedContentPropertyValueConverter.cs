using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PropertyEditors.ValueConverters;
using Umbraco.Web.PublishedCache;

namespace PassleSync.Core.PropertyValueConverters
{
    public class PassleNestedContentPropertyValueConverter : NestedContentManyValueConverter
    {
        public PassleNestedContentPropertyValueConverter(
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            IPublishedModelFactory publishedModelFactory,
            IProfilingLogger proflog
        ) : base(publishedSnapshotAccessor, publishedModelFactory, proflog)
        { }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.InvariantEquals("PassleSync.NestedContent");
        }
    }
}
