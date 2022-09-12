using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
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

        public PassleDashboardPostsController(ISyncHandler<PasslePost> postHandler)
        {
            _postHandler = postHandler;
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
        public IHttpActionResult SyncAll()
        {
            try
            {
                _postHandler.SyncAll();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult SyncMany([FromBody] ShortcodesModel model)
        {
            try
            {
                _postHandler.SyncMany(model.Shortcodes.ToArray());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult SyncOne([FromBody] ShortcodesModel model)
        {
            try
            {
                _postHandler.SyncOne(model.Shortcodes.FirstOrDefault());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteAll()
        {
            try
            {
                _postHandler.DeleteAll();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteMany([FromBody] ShortcodesModel model)
        {
            try
            {
                _postHandler.DeleteMany(model.Shortcodes.ToArray());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteOne([FromBody] ShortcodesModel model)
        {
            try
            {
                _postHandler.DeleteOne(model.Shortcodes.FirstOrDefault());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
