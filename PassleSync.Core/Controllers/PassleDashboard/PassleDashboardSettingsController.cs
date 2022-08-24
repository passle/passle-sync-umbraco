using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models;
using PassleSync.Core.Services;
using PassleSync.Core.ViewModels.PassleDashboard;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardSettingsController : UmbracoAuthorizedJsonController
    {
        private readonly ConfigService _configService;
        protected readonly ILogger _logger;

        public PassleDashboardSettingsController(
            ConfigService configService,
            ILogger logger)
        {
            _configService = configService;
            _logger = logger;
        }

        public PassleDashboardSettingsViewModel Get()
        {
            return new PassleDashboardSettingsViewModel
            {
                PassleShortcodes = _configService.PassleShortcodesString,
                ClientApiKey = _configService.ClientApiKey,
                PluginApiKey = _configService.PluginApiKey,
                PostPermalinkPrefix = _configService.PostPermalinkPrefix,
                AuthorPermalinkPrefix = _configService.AuthorPermalinkPrefix,
                PostsParentNodeId = _configService.PostsParentNodeId,
                AuthorsParentNodeId = _configService.AuthorsParentNodeId,
            };
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody] SettingsModel settings)
        {
            _configService.Update(
                new SettingsData()
                {
                    PassleShortcodes = settings.PassleShortcodes.Split(','),
                    ClientApiKey = settings.ClientApiKey,
                    PluginApiKey = settings.PluginApiKey,
                    PostPermalinkPrefix = settings.PostPermalinkPrefix,
                    AuthorPermalinkPrefix = settings.AuthorPermalinkPrefix,
                    PostsParentNodeId = settings.PostsParentNodeId,
                    AuthorsParentNodeId = settings.AuthorsParentNodeId
                }
            );

            return Ok();
        }
    }
}
