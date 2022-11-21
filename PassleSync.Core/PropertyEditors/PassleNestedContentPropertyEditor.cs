using System;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

namespace PassleSync.Core.PropertyEditors
{
    [DataEditor("PassleSync.NestedContent", "Passle Nested Content", "~/App_Plugins/PassleSync/propertyEditors/nestedContent/nestedContent.html", ValueType = "JSON", Group = "Passle", Icon = "icon-thumbnail-list")]
    public class PassleNestedContentPropertyEditor : NestedContentPropertyEditor
    {
        public PassleNestedContentPropertyEditor(ILogger logger, Lazy<PropertyEditorCollection> propertyEditors, IDataTypeService dataTypeService, IContentTypeService contentTypeService)
            : base(logger, propertyEditors, dataTypeService, contentTypeService)
        {
        }
    }
}