using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using System.Web.Http;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using PassleSync.Core.ViewModels.PassleDashboard;
using System.Collections.Generic;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardAuthorsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<PassleAuthor> _authorHandler;

        public PassleDashboardAuthorsController(ISyncHandler<PassleAuthor> authorHandler)
        {
            _authorHandler = authorHandler;
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

        [HttpPost]
        public IPassleDashboardViewModel SyncAll()
        {
            try
            {
                // Return all authors that were synced
                var syncResults = _authorHandler.SyncAll();

                // Create viewmodels
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardAuthorViewModel(x.Content));
                return new PassleDashboardAuthorsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel SyncMany([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResults = _authorHandler.SyncMany(model.Shortcodes.ToArray());

                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardAuthorViewModel(x.Content));
                return new PassleDashboardAuthorsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _authorHandler.SyncOne(model.Shortcodes.FirstOrDefault());

                if (syncResult.Success)
                {
                    var umbracoPostModels = new List<PassleDashboardAuthorViewModel>
                    {
                        new PassleDashboardAuthorViewModel(syncResult.Content)
                    };
                    return new PassleDashboardAuthorsViewModel(umbracoPostModels);
                }
                else
                {
                    return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
                }
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel DeleteAll()
        {
            try
            {
                var syncResults = _authorHandler.DeleteAll();

                // Return the author's details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardAuthorViewModel(x.Content));
                return new PassleDashboardAuthorsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel DeleteMany([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResults = _authorHandler.DeleteMany(model.Shortcodes.ToArray());

                // Return the author's details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                var umbracoPostModels = syncResults.Where(x => x.Success).Select(x => new PassleDashboardAuthorViewModel(x.Content));
                return new PassleDashboardAuthorsViewModel(umbracoPostModels);
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }

        [HttpPost]
        public IPassleDashboardViewModel DeleteOne([FromBody] ShortcodesModel model)
        {
            try
            {
                var syncResult = _authorHandler.DeleteOne(model.Shortcodes.FirstOrDefault());

                // Return the author's details as they were before they were deleted (but with Synced = false)
                // So we can still display them in the table without updating the API
                if (syncResult.Success)
                {
                    var umbracoPostModels = new List<PassleDashboardAuthorViewModel>
                    {
                        new PassleDashboardAuthorViewModel(syncResult.Content)
                    };
                    return new PassleDashboardAuthorsViewModel(umbracoPostModels);
                }
                else
                {
                    return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
                }
            }
            catch (Exception)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }
        }
    }
}
