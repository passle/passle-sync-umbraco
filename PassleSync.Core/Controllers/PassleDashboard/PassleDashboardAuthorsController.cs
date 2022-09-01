using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using System.Web.Http;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;

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
        public IPassleDashboardViewModel GetAll()
        {
            return _authorHandler.GetAll();
        }

        [HttpPost]
        public IHttpActionResult SyncAll()
        {
            try
            {
                _authorHandler.SyncAll();
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
                _authorHandler.SyncMany(model.Shortcodes.ToArray());
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
                _authorHandler.DeleteAll();
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
                _authorHandler.DeleteMany(model.Shortcodes.ToArray());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
