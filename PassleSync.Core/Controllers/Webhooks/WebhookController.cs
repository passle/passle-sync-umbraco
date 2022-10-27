using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Attributes;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.PassleApi;
using System;
using PassleSync.Core.Services;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using PassleSync.Core.Controllers.ResponseModels;

namespace PassleSync.Core.Controllers
{
    [PluginController("Passle")]
    public class WebhookController : UmbracoApiController
    {
        public ISyncHandler<PasslePost> _postHandler;
        public ISyncHandler<PassleAuthor> _authorHandler;
        protected readonly ConfigService _configService;

        public WebhookController(
            ISyncHandler<PasslePost> postHandler,
            ISyncHandler<PassleAuthor> authorHandler,
            ConfigService configService)
        {
            _postHandler = postHandler;
            _authorHandler = authorHandler;
            _configService = configService;
        }

        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Webhook(WebhookModel model)
        {
            try
            {
                switch (model.Action)
                {
                    case WebhookAction.SYNC_POST:
                        _postHandler.SyncOne(model.Data["Shortcode"]);
                        return Ok();
                    case WebhookAction.DELETE_POST:
                        _postHandler.DeleteOne(model.Data["Shortcode"]);
                        return Ok();
                    case WebhookAction.SYNC_AUTHOR:
                        _authorHandler.SyncOne(model.Data["Shortcode"]);
                        return Ok();
                    case WebhookAction.DELETE_AUTHOR:
                        _authorHandler.DeleteOne(model.Data["Shortcode"]);
                        return Ok();
                    case WebhookAction.UPDATE_FEATURED_POST:
                        _postHandler.UpdateFeaturedContent(model.Data["Shortcode"], model.Data["IsFeaturedOnPasslePage"] == "True", model.Data["IsFeaturedOnPostPage"] == "True");
                        return Ok();
                    case WebhookAction.PING:
                        var postPrefix = _configService.PostPermalinkPrefix;
                        var authorPrefix = _configService.AuthorPermalinkPrefix;
                        return Ok(new PingResponseModel(postPrefix, authorPrefix));
                    default:
                        return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }


}
