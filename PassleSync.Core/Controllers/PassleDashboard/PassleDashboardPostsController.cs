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
using System.Collections;
using System.Collections.Generic;
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
        public IPassleDashboardViewModel SyncAll()
        {
            try
            {
                //// Return all posts that were synced
                //var syncResults = _postHandler.SyncAll();

                //// Create viewmodels
                //var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
                //return new PassleDashboardPostsViewModel(umbracoPostModels);

                var umbracoPosts = (PassleDashboardPostsViewModel) _postHandler.GetAll();
                _backgroundSyncService.AddItemsToSync(umbracoPosts.Posts.Select(x => x.Shortcode));

                return umbracoPosts;
            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel SyncMany([FromBody] ShortcodesModel model)
        {
            try
            {
                //var syncResults = _postHandler.SyncMany(model.Shortcodes.ToArray());

                //var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
                //return new PassleDashboardPostsViewModel(umbracoPostModels);

                _backgroundSyncService.AddItemsToSync(model.Shortcodes);

                var umbracoPosts = _umbracoContentService.GetAllContent().Where(x => model.Shortcodes.Contains(_umbracoContentService.Shortcode(x)));
                var umbracoPostModels = umbracoPosts.Select(x => new PassleDashboardPostViewModel(x));
                return new PassleDashboardPostsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _postHandler.SyncOne(model.Shortcodes.FirstOrDefault());

                if (syncResult.Success)
                {
                    var umbracoPostModels = new List<PassleDashboardPostViewModel>
                    {
                        new PassleDashboardPostViewModel(syncResult.Content)
                    };
                    return new PassleDashboardPostsViewModel(umbracoPostModels);
                }
                else
                {
                    return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
                }
            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }

       [HttpPost]
        public IPassleDashboardViewModel DeleteAll()
        {
            try
            {
                var umbracoPosts = _umbracoContentService.GetAllContent();
                _backgroundSyncService.AddItemsToDelete(umbracoPosts.Select(x => x.GetValue<string>("postShortcode")));

                // Return the post details as they were before they were deleted (but with Synced = false)
                // In case the front-end needs it
                var umbracoPostModels = umbracoPosts.Select(x => new PassleDashboardPostViewModel(x));
                return new PassleDashboardPostsViewModel(umbracoPostModels);

            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel DeleteMany([FromBody] ShortcodesModel model)
        {
            try
            {
                //var syncResults = _postHandler.DeleteMany(model.Shortcodes.ToArray());

                //// Return the post details as they were before they were deleted (but with Synced = false)
                //// So we can still display them in the table without updating the API
                //var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
                //return new PassleDashboardPostsViewModel(umbracoPostModels);

                _backgroundSyncService.AddItemsToDelete(model.Shortcodes);

                var umbracoPosts = _umbracoContentService.GetAllContent().Where(x => model.Shortcodes.Contains(_umbracoContentService.Shortcode(x)));
                var umbracoPostModels = umbracoPosts.Select(x => new PassleDashboardPostViewModel(x));
                return new PassleDashboardPostsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel DeleteOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _postHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

                // Return the post details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                if (syncResult.Success)
                {
                    var umbracoPostModels = new List<PassleDashboardPostViewModel>
                    {
                        new PassleDashboardPostViewModel(syncResult.Content)
                    };
                    return new PassleDashboardPostsViewModel(umbracoPostModels);
                }
                else
                {
                    return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
                }
            }
            catch (Exception)
            {
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }
        }
    }
}
