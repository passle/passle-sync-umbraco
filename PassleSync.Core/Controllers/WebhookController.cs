using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;
using System.Web.Http;
using Umbraco.Core.Services;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace PassleSync.Core.Controllers
{
    public class MyController : Umbraco.Web.WebApi.UmbracoApiController
    {
        public IContentService _contentService;
        public ISyncHandler<Post> _postHandler;
        public ISyncHandler<Person> _personHandler;

        public MyController(IContentService contentService, ISyncHandler<Post> postHandler)
        {
            _contentService = contentService;
            _postHandler = postHandler;
        }


        [HttpPost]
        public IHttpActionResult SyncPost(PostShortcodeModel post)
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


        [HttpPost]
        public IHttpActionResult SyncAuthor(AuthorShortcodeModel author)
        {
            if (_personHandler.SyncOne(author.Shortcode))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }

    public class SyncableObjectModel
    {
        public string Shortcode { get; set; }
    }

    public class PostShortcodeModel : SyncableObjectModel
    { }

    public class AuthorShortcodeModel : SyncableObjectModel
    { }
}
