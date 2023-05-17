using Microsoft.CodeAnalysis.CSharp.Syntax;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Controllers.ResponseModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;
using PassleSync.Core.SyncHandlers;
using PassleSync.Core.ViewModels.PassleDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardBaseSyncController<T> : UmbracoAuthorizedJsonController where T: class 
    {
        protected readonly ISyncHandler<T> _syncHandler;
        protected readonly UmbracoContentService<T> _umbracoContentService;
        protected readonly BackgroundSyncServiceBase<T> _backgroundSyncService;

        public PassleDashboardBaseSyncController(
            ISyncHandler<T> syncHandler,
            UmbracoContentService<T> umbracoContentService,
            BackgroundSyncServiceBase<T> backgroundSyncService
        )
        {
            _syncHandler = syncHandler;
            _umbracoContentService = umbracoContentService;
            _backgroundSyncService = backgroundSyncService;
        }

        [HttpGet]
        public IPassleDashboardViewModel RefreshAll()
        {
            return _syncHandler.GetAll();
        }

        [HttpGet]
        public IPassleDashboardViewModel GetExisting()
        {
            return _syncHandler.GetExisting();
        }

        [HttpGet]
        public SyncStatusResponseModel GetPending()
        {
            var toSync = _backgroundSyncService.GetItemsToSync().ToList();
            var toDelete = _backgroundSyncService.GetItemsToDelete().ToList();
            return new SyncStatusResponseModel(toSync, toDelete);
        }

        [HttpPost]
        public IHttpActionResult SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _syncHandler.SyncOne(model.Shortcodes.FirstOrDefault());

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
        public IHttpActionResult SyncAll()
        {
            try
            {
                if (typeof(T) == typeof(PasslePost))
                {
                    var umbracoPosts = (PassleDashboardPostsViewModel)_syncHandler.GetAll();
                    _backgroundSyncService.AddItemsToSync(umbracoPosts.Posts.Select(x => x.Shortcode));
                } 
                else if (typeof(T) == typeof(PassleAuthor))
                {
                    var umbracoAuthors = (PassleDashboardAuthorsViewModel)_syncHandler.GetAll();
                    _backgroundSyncService.AddItemsToSync(umbracoAuthors.Authors.Select(x => x.Shortcode));
                }
                else
                {
                    throw new InvalidOperationException("Unknown type of entities to sync.");
                }
                
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
                var syncResult = _syncHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

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
        public IHttpActionResult DeleteAll()
        {
            try
            {
                if (typeof(T) == typeof(PasslePost))
                {
                    var umbracoPosts = _umbracoContentService.GetAllContent();
                    _backgroundSyncService.AddItemsToDelete(umbracoPosts.Select(x => x.GetValue<string>("postShortcode")));
                }
                else if (typeof(T) == typeof(PassleAuthor))
                {
                    var umbracoAuthors = _umbracoContentService.GetAllContent();
                    _backgroundSyncService.AddItemsToDelete(umbracoAuthors.Select(x => x.GetValue<string>("shortcode")));
                }
                else
                {
                    throw new InvalidOperationException("Unknown type of entities to delete.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
