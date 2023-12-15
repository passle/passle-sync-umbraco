using PassleSync.Core.Constants;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace PassleSync.Core.PropertyEditors
{
    [TagsPropertyEditorAttribute]
    [DataEditor(PassleDataType.TAGS, PassleDataType.TAGS, "~/App_Plugins/PassleSync/propertyEditors/tags/tags.html", Group = "Passle", Icon = "icon-checkbox")]
    public class PassleTagsPropertyEditor : TagsPropertyEditor
    {
        public PassleTagsPropertyEditor(ManifestValueValidatorCollection validators, ILogger logger)
            : base(validators, logger)
        {
        }
    }
}