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
    public class PostsController : UmbracoApiController
    {
        public ISyncHandler<PasslePost> _postHandler;

        public PostsController(ISyncHandler<PasslePost> postHandler)
        {
            _postHandler = postHandler;
        }


        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Update(PostShortcodeModel post)
        {
            if (_postHandler.SyncOne(post.Shortcode))
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
