using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Dashboards;

namespace PassleSync.Core.Dashboards
{
    //[Weight(-10)]
    public class PassleSync : IDashboard
    {
        public string Alias => "PassleSync";

        public string[] Sections => new[]
        {
            Umbraco.Core.Constants.Applications.Content,
            Umbraco.Core.Constants.Applications.Members,
            Umbraco.Core.Constants.Applications.Settings
        };

        public string View => "/App_Plugins/PassleSync/dashboard.html";

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();       
    }
}
