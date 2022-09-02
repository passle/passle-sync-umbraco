using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Attributes;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace PassleSync.Core.Controllers
{
    [PluginController("Passle")]
    public class PeopleController : UmbracoApiController
    {
        public ISyncHandler<PassleAuthor> _personHandler;

        public PeopleController(ISyncHandler<PassleAuthor> personHandler)
        {
            _personHandler = personHandler;
        }


        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Update([FromBody] AuthorShortcodeModel model)
        {
            if (_personHandler.SyncOne(model.Shortcode))
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
