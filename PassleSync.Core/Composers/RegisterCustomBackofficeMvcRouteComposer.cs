using PassleSync.Core.Components;
using PassleSync.Core.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Composers
{
    public class RegisterCustomBackofficeMvcRouteComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<MyPassleSyncController>(Lifetime.Request);
            composition.Components().Append<RegisterPassleBackofficeDashboardMvcRouteComponent>();
        }
    }
}
