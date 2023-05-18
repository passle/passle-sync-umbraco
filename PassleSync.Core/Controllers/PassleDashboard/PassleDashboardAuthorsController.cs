using Umbraco.Web.Mvc;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardAuthorsController : PassleDashboardBaseSyncController<PassleAuthor>
    {
        protected override ISyncHandler<PassleAuthor> SyncHandler { get; set; }
        protected override UmbracoContentService<PassleAuthor> UmbracoContentService { get; set; }
        protected override BackgroundSyncServiceBase<PassleAuthor> BackgroundSyncService { get; set; }

        public PassleDashboardAuthorsController(
            ISyncHandler<PassleAuthor> authorHandler,
            UmbracoContentService<PassleAuthor> umbracoContentService,
            BackgroundSyncServiceBase<PassleAuthor> backgroundSyncService
        )
        { 
            SyncHandler = authorHandler;
            UmbracoContentService = umbracoContentService;
            BackgroundSyncService = backgroundSyncService;
        }
    }
}
