using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Attributes;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using PassleSync.Core.Services;
using System.Linq;
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
        private readonly ConfigService _configService;

        public PostsController(ISyncHandler<PasslePost> postHandler, ConfigService configService)
        {
            _postHandler = postHandler;
            _configService = configService;
        }

        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Update(PostShortcodeModel model)
        {
            try
            {
                if (!_configService.PassleShortcodes.Contains(model.PassleShortcode))
                {
                    return Ok();
                }

                _postHandler.SyncOne(model.PostShortcode);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
