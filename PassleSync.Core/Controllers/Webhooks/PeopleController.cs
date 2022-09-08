using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Attributes;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace PassleSync.Core.Controllers
{
    [PluginController("Passle")]
    public class PeopleController : UmbracoApiController
    {
        public ISyncHandler<PassleAuthor> _authorHandler;

        public PeopleController(ISyncHandler<PassleAuthor> authorHandler)
        {
            _authorHandler = authorHandler;
        }


        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Update([FromBody] AuthorShortcodeModel model)
        {
            try
            {
                _authorHandler.SyncOne(model.Shortcode);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
