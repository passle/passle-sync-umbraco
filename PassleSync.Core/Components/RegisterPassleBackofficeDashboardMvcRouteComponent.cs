using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;

namespace PassleSync.Core.Components
{
    public class RegisterPassleBackofficeDashboardMvcRouteComponent : IComponent
    {
        private readonly IGlobalSettings _globalSettings;

        public RegisterPassleBackofficeDashboardMvcRouteComponent(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public void Initialize()
        {
            RouteTable.Routes.MapRoute("myPassleSync",
                _globalSettings.GetUmbracoMvcArea() + "/backoffice/myPassleSync/myPassleSync/index",
                new
                {
                    controller = "myPassleSync",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                constraints: new { controller = "myPassleSync" });
        }

        public void Terminate()
        {
            // Nothing to terminate
        }
    }
}
