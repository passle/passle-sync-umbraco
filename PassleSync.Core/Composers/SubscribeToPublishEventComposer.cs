using PassleSync.Core.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SubscribeToPublishEventComposer : ComponentComposer<SubscribeToPublishEventComponent>
    { }
}
