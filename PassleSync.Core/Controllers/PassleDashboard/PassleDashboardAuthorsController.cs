using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;
using System.Web.Http;
using Passle.BackOffice.Controllers.RequestModels;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;
using PassleSync.Core.API.ViewModels;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardAuthorsController : UmbracoAuthorizedJsonController
    {
        private readonly ISyncHandler<Person> _authorHandler;
        protected readonly ILogger _logger;

        public PassleDashboardAuthorsController(
            ISyncHandler<Person> authorHandler,
            ILogger logger)
        {
            _authorHandler = authorHandler;
            _logger = logger;
        }

        public IPassleDashboardViewModel RefreshAll()
        {
            return _authorHandler.GetAll();
        }
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
            //var posts = await _passleContentService.GetPosts(model.Shortcodes);
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
