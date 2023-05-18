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
        protected override ISyncHandler<PasslePost> SyncHandler { get; set; }
        protected override UmbracoContentService<PasslePost> UmbracoContentService { get; set; }
        protected override BackgroundSyncServiceBase<PasslePost> BackgroundSyncService { get; set; }

        public PassleDashboardPostsController(
            ISyncHandler<PasslePost> postHandler,
            UmbracoContentService<PasslePost> umbracoContentService,
            BackgroundSyncServiceBase<PasslePost> backgroundSyncService
        )
        {
            SyncHandler = postHandler;
            UmbracoContentService = umbracoContentService;
            BackgroundSyncService = backgroundSyncService;
        }
    }
}
