using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;
using UmbracoConstants = Umbraco.Core.Constants;
using ModelBinderAttribute = System.Web.Http.ModelBinding.ModelBinderAttribute;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [Tree("settings", "passleSync", TreeTitle = "Passle Sync", TreeGroup = UmbracoConstants.Trees.Groups.Settings, SortOrder = 30)]
    [PluginController("PassleSync")]
    public class PassleDashboardTreeController : TreeController
    {
        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            var result = base.CreateRootNode(queryStrings);

            //result.RoutePath = $"{SectionAlias}/passleSync/dashboard";

            // TODO: Use constants instead of magic strings
            result.RoutePath = string.Format($"{UmbracoConstants.Applications.Settings}/passleSync/dashboard");
            result.HasChildren = false;
            result.MenuUrl = null;
            result.Icon = "icon-sync";

            return result;
        }

        protected override MenuItemCollection GetMenuForNode(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {
            throw new System.NotImplementedException();
        }

        protected override TreeNodeCollection GetTreeNodes(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {
            throw new System.NotImplementedException();
        }
    }
}
