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
        public PassleDashboardAuthorsController(
            ISyncHandler<PassleAuthor> authorHandler,
            UmbracoContentService<PassleAuthor> umbracoContentService,
            BackgroundSyncServiceBase<PassleAuthor> backgroundSyncService
        )
            :base(authorHandler, umbracoContentService, backgroundSyncService)
        { }
    }
}
