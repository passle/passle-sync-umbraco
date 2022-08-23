using SubscribeToPublishEventComposer;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleDotCom.PasslePlugin.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SubscribeToPublishEventComposer : ComponentComposer<SubscribeToPublishEventComponent>
    { }
}