using Passle.BackOffice.Controllers.RequestModels;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Admin;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardPostsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<Post> _postHandler;
        protected readonly ILogger _logger;

        public PassleDashboardPostsController(
            ISyncHandler<Post> postHandler,
            ILogger logger)
        {
            _postHandler = postHandler;
            _logger = logger;
        }

        [HttpGet]
        public IPassleDashboardViewModel RefreshAll()
        {
            return _postHandler.GetAll();
        }

        [HttpGet]
        public IPassleDashboardViewModel GetAll()
        {
            return _postHandler.GetAll();
        }

        [HttpPost]
        public IHttpActionResult SyncAll()
        {
            if (_postHandler.SyncAll())
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
            if (_postHandler.SyncMany(model.Shortcodes.ToArray()))
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
            if (_postHandler.DeleteAll())
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
            if (_postHandler.DeleteMany(model.Shortcodes.ToArray()))
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
