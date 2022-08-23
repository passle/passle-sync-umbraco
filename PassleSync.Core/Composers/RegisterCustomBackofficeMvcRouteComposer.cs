using PassleDotCom.PasslePlugin.Core.Components;
using PassleDotCom.PasslePlugin.Core.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleDotCom.PasslePlugin.Core.Composers
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
