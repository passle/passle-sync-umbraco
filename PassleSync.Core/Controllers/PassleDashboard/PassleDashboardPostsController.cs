using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Controllers.ResponseModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;
using PassleSync.Core.ViewModels.PassleDashboard;
using System;
using System.Linq;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardPostsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<PasslePost> _postHandler;
        protected readonly UmbracoContentService<PasslePost> _umbracoContentService;
        protected readonly BackgroundSyncServiceBase<PasslePost> _backgroundSyncService;

        public PassleDashboardPostsController(
            ISyncHandler<PasslePost> postHandler,
            UmbracoContentService<PasslePost> umbracoContentService,
            BackgroundSyncServiceBase<PasslePost> backgroundSyncService
        )
        {
            _postHandler = postHandler;
            _umbracoContentService = umbracoContentService;
            _backgroundSyncService = backgroundSyncService;
        }

        [HttpGet]
        public IPassleDashboardViewModel RefreshAll()
        {
            return _postHandler.GetAll();
        }

        [HttpGet]
        public IPassleDashboardViewModel GetExisting()
        {
            return _postHandler.GetExisting();
        }

        [HttpGet]
        public SyncStatusResponseModel GetPending()
        {
            var toSync = _backgroundSyncService.GetItemsToSync().ToList();
            var toDelete = _backgroundSyncService.GetItemsToDelete().ToList();
            return new SyncStatusResponseModel(toSync, toDelete);
        }

        [HttpPost]
        public IHttpActionResult SyncAll()
        {
            try
            {
                var umbracoPosts = (PassleDashboardPostsViewModel) _postHandler.GetAll();
                _backgroundSyncService.AddItemsToSync(umbracoPosts.Posts.Select(x => x.Shortcode));

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult SyncMany([FromBody] ShortcodesModel model)
        {
            try
            {
                _backgroundSyncService.AddItemsToSync(model.Shortcodes);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _postHandler.SyncOne(model.Shortcodes.FirstOrDefault());

                if (syncResult.Success)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

       [HttpPost]
        public IHttpActionResult DeleteAll()
        {
            try
            {
                var umbracoPosts = _umbracoContentService.GetAllContent();
                _backgroundSyncService.AddItemsToDelete(umbracoPosts.Select(x => x.GetValue<string>("postShortcode")));
                
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteMany([FromBody] ShortcodesModel model)
        {
            try
            {
                _backgroundSyncService.AddItemsToDelete(model.Shortcodes);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _postHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

                if (syncResult.Success)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
