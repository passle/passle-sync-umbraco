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
using PassleSync.Core.Actions;

namespace PassleSync.Core.Controllers
{
    [PluginController("Passle")]
    public class WebhookController : UmbracoApiController
    {
        public ISyncHandler<PasslePost> _postHandler;
        public ISyncHandler<PassleAuthor> _authorHandler;
        public UpdateFeaturedPostAction _updateFeaturedPostAction;
        protected readonly ConfigService _configService;

        public WebhookController(
            ISyncHandler<PasslePost> postHandler,
            ISyncHandler<PassleAuthor> authorHandler,
            UpdateFeaturedPostAction updateFeaturedPostAction,
            ConfigService configService)
        {
            _postHandler = postHandler;
            _authorHandler = authorHandler;
            _updateFeaturedPostAction = updateFeaturedPostAction;
            _configService = configService;
        }

        [HttpPost]
        [ValidateAPIKey]
        public IHttpActionResult Handle(WebhookModel model)
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
                    var actionModel = new UpdateFeaturedPostActionModel(model.Data["Shortcode"], model.Data["IsFeaturedOnPasslePage"], model.Data["IsFeaturedOnPostPage"]);
                    _updateFeaturedPostAction.Execute(actionModel);
                    return Ok();
                case WebhookAction.PING:
                    var postTemplate = _configService.PostPermalinkTemplate;
                    var personTemplate = _configService.PersonPermalinkTemplate;
                    return Ok(new PingResponseModel(postTemplate, personTemplate));
                default:
                    return BadRequest("The action specified is not supported");
            }
        }
    }
}
