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
    public abstract class PassleDashboardBaseSyncController<T>: UmbracoAuthorizedJsonController where T : class
    {
        protected abstract ISyncHandler<T> SyncHandler { get; set; }
        protected abstract UmbracoContentService<T> UmbracoContentService { get; set; }
        protected abstract BackgroundSyncServiceBase<T> BackgroundSyncService { get; set; }

        [HttpGet]
        public IPassleDashboardViewModel RefreshAll()
        {
            return SyncHandler.GetAll();
        }

        [HttpGet]
        public IPassleDashboardViewModel GetExisting()
        {
            return SyncHandler.GetExisting();
        }

        [HttpGet]
        public SyncStatusResponseModel GetPending()
        {
            var toSync = BackgroundSyncService.GetItemsToSync().ToList();
            var toDelete = BackgroundSyncService.GetItemsToDelete().ToList();
            return new SyncStatusResponseModel(toSync, toDelete);
        }

        [HttpPost]
        public IHttpActionResult SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = SyncHandler.SyncOne(model.Shortcodes.FirstOrDefault());

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
                BackgroundSyncService.AddItemsToSync(model.Shortcodes);

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
                    var umbracoPosts = (PassleDashboardPostsViewModel)SyncHandler.GetAll();
                    BackgroundSyncService.AddItemsToSync(umbracoPosts.Posts.Select(x => x.Shortcode));
                } 
                else if (typeof(T) == typeof(PassleAuthor))
                {
                    var umbracoAuthors = (PassleDashboardAuthorsViewModel)SyncHandler.GetAll();
                    BackgroundSyncService.AddItemsToSync(umbracoAuthors.Authors.Select(x => x.Shortcode));
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
                var syncResult = SyncHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

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
                BackgroundSyncService.AddItemsToDelete(model.Shortcodes);

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
                    var umbracoPosts = UmbracoContentService.GetAllContent();
                    BackgroundSyncService.AddItemsToDelete(umbracoPosts.Select(x => x.GetValue<string>("postShortcode")));
                }
                else if (typeof(T) == typeof(PassleAuthor))
                {
                    var umbracoAuthors = UmbracoContentService.GetAllContent();
                    BackgroundSyncService.AddItemsToDelete(umbracoAuthors.Select(x => x.GetValue<string>("shortcode")));
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
