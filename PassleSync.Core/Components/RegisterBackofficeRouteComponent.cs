using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;

namespace PassleSync.Core.Components
{
    public class RegisterBackofficeRouteComponent : IComponent
    {
        private readonly IGlobalSettings _globalSettings;

        public RegisterBackofficeRouteComponent(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public void Initialize()
        {
            RouteTable.Routes.MapRoute("passleSync",
                _globalSettings.GetUmbracoMvcArea() + "/backoffice/passleSync/index",
                new
                {
                    controller = "passleSync",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                constraints: new { controller = "passleSync" });
        }

        public void Terminate()
        {
            // Nothing to terminate
        }
    }
}
