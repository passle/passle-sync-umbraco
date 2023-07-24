using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.Umbraco;
using PassleSync.Core.Services;
using PassleSync.Core.ViewModels.PassleDashboard;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardSettingsController : UmbracoAuthorizedJsonController
    {
        private readonly ConfigService _configService;
        private readonly IContentService _contentService;
        protected readonly ILogger _logger;

        public PassleDashboardSettingsController(
            IContentService contentService,
            ConfigService configService,
            ILogger logger)
        {
            _configService = configService;
            _contentService = contentService;
            _logger = logger;
        }

        public PassleDashboardSettingsViewModel Get()
        {
            return new PassleDashboardSettingsViewModel
            {
                PassleShortcodes = _configService.PassleShortcodesString,
                ClientApiKey = _configService.ClientApiKey,
                PluginApiKey = _configService.PluginApiKey,
                PostPermalinkTemplate = _configService.PostPermalinkTemplate,
                PersonPermalinkTemplate = _configService.PersonPermalinkTemplate,
                PostsParentNodeId = _configService.PostsParentNodeId,
                PreviewPermalinkTemplate = _configService.PreviewPermalinkTemplate,
                AuthorsParentNodeId = _configService.AuthorsParentNodeId,
                DomainExt = _configService.PassleDomain,
            };
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody] SettingsModel settings)
        {
            var postParentNode = _contentService.GetById(settings.PostsParentNodeId);
            if (postParentNode == null)
            {
                return BadRequest("Post Parent Node doesn't exist");
            }

            var authorsParentNode = _contentService.GetById(settings.AuthorsParentNodeId);
            if (authorsParentNode == null)
            {
                return BadRequest("Author Parent Node doesn't exist");
            }

            _configService.Update(
                new SettingsData()
                {
                    PassleShortcodes = settings.PassleShortcodes.Split(','),
                    ClientApiKey = settings.ClientApiKey,
                    PluginApiKey = settings.PluginApiKey,
                    PostPermalinkTemplate = settings.PostPermalinkTemplate,
                    PersonPermalinkTemplate = settings.PersonPermalinkTemplate,
                    PreviewPermalinkTemplate = settings.PreviewPermalinkTemplate,
                    PostsParentNodeId = settings.PostsParentNodeId,
                    AuthorsParentNodeId = settings.AuthorsParentNodeId
                }
            );

            return Ok();
        }
    }
}
