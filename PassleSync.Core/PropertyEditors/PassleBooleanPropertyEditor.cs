using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace PassleSync.Core.PropertyEditors
{
    [DataEditor("Passle Boolean", EditorType.PropertyValue | EditorType.MacroParameter, "Passle Boolean", "~/App_Plugins/PassleSync/propertyEditors/boolean/boolean.html", ValueType = ValueTypes.Integer, Group = "Passle", Icon = "icon-checkbox")]
    public class PassleBooleanPropertyEditor : TrueFalsePropertyEditor
    {
        public PassleBooleanPropertyEditor(ILogger logger)
            : base(logger)
        {
        }
    }
}