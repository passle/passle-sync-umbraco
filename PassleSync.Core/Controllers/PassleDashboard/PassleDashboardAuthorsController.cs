using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using System.Web.Http;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Controllers.RequestModels;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardAuthorsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<Person> _authorHandler;

        public PassleDashboardAuthorsController(ISyncHandler<Person> authorHandler)
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
            if (_authorHandler.SyncAll())
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult SyncMany([FromBody] ShortcodesModel model)
        {
            if (_authorHandler.SyncMany(model.Shortcodes.ToArray()))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteAll()
        {
            if (_authorHandler.DeleteAll())
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteMany([FromBody] ShortcodesModel model)
        {
            if (_authorHandler.DeleteMany(model.Shortcodes.ToArray()))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
