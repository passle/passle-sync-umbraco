using PassleSync.Core.Constants;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace PassleSync.Core.PropertyEditors
{
    [DataEditor(PassleDataType.BOOLEAN, EditorType.PropertyValue | EditorType.MacroParameter, PassleDataType.BOOLEAN, "~/App_Plugins/PassleSync/propertyEditors/boolean/boolean.html", ValueType = ValueTypes.Integer, Group = "Passle", Icon = "icon-checkbox")]
    public class PassleBooleanPropertyEditor : TrueFalsePropertyEditor
    {
        public PassleBooleanPropertyEditor(ILogger logger)
            : base(logger)
        {
        }
    }
}