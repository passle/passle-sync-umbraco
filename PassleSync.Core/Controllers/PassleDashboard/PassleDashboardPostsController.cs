using Umbraco.Web.Mvc;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardPostsController : PassleDashboardBaseSyncController<PasslePost>
    {
        public PassleDashboardPostsController(
            ISyncHandler<PasslePost> postHandler,
            UmbracoContentService<PasslePost> umbracoContentService,
            BackgroundSyncServiceBase<PasslePost> backgroundSyncService
        )
            :base(postHandler, umbracoContentService, backgroundSyncService)
        { }
    }
}
