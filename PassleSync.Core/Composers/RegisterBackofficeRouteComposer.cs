using PassleSync.Core.Components;
using PassleSync.Core.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Composers
{
    public class RegisterBackofficeRouteComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<PassleSyncController>(Lifetime.Request);
            composition.Components().Append<RegisterBackofficeRouteComponent>();
        }
    }
}
