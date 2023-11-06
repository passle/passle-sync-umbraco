using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Models.Content.Umbraco;
using PassleSync.Core.Services;
using PassleSync.Core.ViewModels.PassleDashboard;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                PostPermalinkTemplate = _configService.PostPermalinkTemplate ?? "p/{{PostShortcode}}/{{PostSlug}}",
                PersonPermalinkTemplate = _configService.PersonPermalinkTemplate ?? "u/{{PersonShortcode}}/{{PersonSlug}}",
                PreviewPermalinkTemplate = _configService.PreviewPermalinkTemplate,
                SimulateRemoteHosting = _configService.SimulateRemoteHosting,
                PostsParentNodeId = _configService.PostsParentNodeId,
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

            if (!settings.PostPermalinkTemplate.Contains("{{PostShortcode}}")) 
            {
                return BadRequest("The post permalink template must contain the {{PostShortcode}} variable");
            }

            if (!settings.PersonPermalinkTemplate.Contains("{{PersonShortcode}}")) 
            {
                return BadRequest("The person permalink template must contain the {{PersonShortcode}} variable");
            }

            if (!string.IsNullOrEmpty(settings.PreviewPermalinkTemplate) && !settings.PreviewPermalinkTemplate.Contains("{{PostShortcode}}")) 
            {
                return BadRequest("The preview permalink template must contain the {{PostShortcode}} variable");
            }

            if (!ValidatePermalinkTemplateUniqueness(settings))
            {
                return BadRequest("Permalink templates must be unique");
            }

            if (!ValidatePermalinkTemplateAllowedVariables(settings))
            {
                return BadRequest("Permalink templates must only contain allowed variables");
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

        private bool ValidatePermalinkTemplateUniqueness(SettingsModel settings)
        {
            var permalinkTemplates = new List<string>
            {
                settings.PostPermalinkTemplate,
                settings.PersonPermalinkTemplate,
                settings.PreviewPermalinkTemplate
            }.Where(x => !string.IsNullOrEmpty(x));

            var regex = new Regex("{{(.*?)}}");
            var uniqueTemplates = permalinkTemplates.Select(x => regex.Replace(x, "")).Distinct().ToList();

            return uniqueTemplates.Count() == permalinkTemplates.Count();
        }

        private bool ValidatePermalinkTemplateAllowedVariables(SettingsModel settings)
        {
            var allowedVariablesBase = new List<string> { "PassleShortcode" };
            var allowedVariablesPost = allowedVariablesBase.Concat(new List<string> { "PostShortcode", "PostSlug" });
            var allowedVariablesPerson = allowedVariablesBase.Concat(new List<string> { "PersonShortcode", "PersonSlug" });
            var allowedVariablesPreview = allowedVariablesPost;

            return ValidateSinglePermalinkTemplate(settings.PostPermalinkTemplate, allowedVariablesPost) &&
                ValidateSinglePermalinkTemplate(settings.PersonPermalinkTemplate, allowedVariablesPerson) &&
                ValidateSinglePermalinkTemplate(settings.PreviewPermalinkTemplate, allowedVariablesPreview);
        }

        private bool ValidateSinglePermalinkTemplate(string template, IEnumerable<string> allowedVariables)
        {
            if (string.IsNullOrEmpty(template))
            {
                return true;
            }

            var permalinkTemplateVariables = new List<string>();
            var regex = new Regex("{{(.*?)}}");
            var matches = regex.Matches(template);

            foreach (Match match in matches)
            {
                permalinkTemplateVariables.Add(match.Groups[1].Value);
            }

            var invalidVariables = permalinkTemplateVariables.Except(allowedVariables).ToList();
            return invalidVariables.Count() == 0;
        }
    }
}
