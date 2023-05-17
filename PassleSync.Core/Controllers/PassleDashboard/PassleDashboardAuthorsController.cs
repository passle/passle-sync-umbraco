using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using System.Web.Http;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using PassleSync.Core.Controllers.ResponseModels;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;
using PassleSync.Core.ViewModels.PassleDashboard;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardAuthorsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<PassleAuthor> _authorHandler;
        protected readonly UmbracoContentService<PassleAuthor> _umbracoContentService;
        protected readonly BackgroundSyncServiceBase<PassleAuthor> _backgroundSyncService;

        public PassleDashboardAuthorsController(
            ISyncHandler<PassleAuthor> authorHandler,
            UmbracoContentService<PassleAuthor> umbracoContentService,
            BackgroundSyncServiceBase<PassleAuthor> backgroundSyncService
        )
        {
            _authorHandler = authorHandler;
            _umbracoContentService = umbracoContentService;
            _backgroundSyncService = backgroundSyncService;
        }

        [HttpGet]
        public IPassleDashboardViewModel RefreshAll()
        {
            return _authorHandler.GetAll();
        }

        [HttpGet]
        public IPassleDashboardViewModel GetExisting()
        {
            return _authorHandler.GetExisting();
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
                var umbracoAuthors = (PassleDashboardAuthorsViewModel)_authorHandler.GetAll();
                _backgroundSyncService.AddItemsToSync(umbracoAuthors.Authors.Select(x => x.Shortcode));

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
                var syncResult = _authorHandler.SyncOne(model.Shortcodes.FirstOrDefault());

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
                var umbracoAuthors = _umbracoContentService.GetAllContent();
                _backgroundSyncService.AddItemsToDelete(umbracoAuthors.Select(x => x.GetValue<string>("shortcode")));

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
                var syncResult = _authorHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

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
