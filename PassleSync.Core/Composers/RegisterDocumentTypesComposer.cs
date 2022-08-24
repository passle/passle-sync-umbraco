using Umbraco.Core.Composing;
using Umbraco.Core;
using PassleDotCom.PasslePlugin.Core.Components;

namespace PassleDotCom.PasslePlugin.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class RegisterDocumentTypesComposer : ComponentComposer<RegisterDocumentTypesComponent>
    {
    }
}
