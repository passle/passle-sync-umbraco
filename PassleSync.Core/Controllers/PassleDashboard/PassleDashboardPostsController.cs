using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.Content;
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

        public PassleDashboardPostsController(
            ISyncHandler<PasslePost> postHandler,
            UmbracoContentService<PasslePost> umbracoContentService
        )
        {
            _postHandler = postHandler;
            _umbracoContentService = umbracoContentService;
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

        [HttpPost]
        public IPassleDashboardViewModel SyncAll()
        {
            try
            {
                // Return all posts that were synced
                var syncResults = _postHandler.SyncAll();

                // Create viewmodels
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
                return new PassleDashboardPostsViewModel(umbracoPostModels);
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
                var syncResults = _postHandler.SyncMany(model.Shortcodes.ToArray());

                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
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
                var syncResults = _postHandler.DeleteAll();

                // Return the post details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
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
                var syncResults = _postHandler.DeleteMany(model.Shortcodes.ToArray());

                // Return the post details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardPostViewModel(x.Content));
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
