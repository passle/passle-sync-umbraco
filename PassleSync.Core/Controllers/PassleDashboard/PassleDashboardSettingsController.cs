using PassleSync.Core.Services;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardSettingsController : UmbracoAuthorizedJsonController
    {
        private readonly PassleContentService _passleContentService;
        private readonly ApiService _apiService;
        protected readonly ILogger _logger;

        public PassleDashboardSettingsController(
            PassleContentService passleContentService,
            ApiService apiService,
            ILogger logger)
        {
            _passleContentService = passleContentService;
            _apiService = apiService;
            _logger = logger;
        }

        public PassleDashboardSettingsViewModel Get()
        {
            return new PassleDashboardSettingsViewModel(
                ConfigService.Passle.APIKey,
                _apiService.ApiKey,
                _passleContentService.PassleShortcodes
            );
        }

        //public void Save(SettingsModel model)
        //{
        //    _apiService.ApiKey = model.APIKey;
        //    _passleContentService.PassleShortcodes = model.Shortcodes.Split(",").Select(x => x.Trim());
        //}
    }
}
