using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardSettingsViewModel : IPassleDashboardViewModel
    {
        public string APIKey;
        public string ClientWebAPIKey;
        public string PassleShortcodes;

        public PassleDashboardSettingsViewModel(string apiKey, string cwApiKey, IEnumerable<string> passleShortcodes)
        {
            APIKey = apiKey;
            ClientWebAPIKey = cwApiKey;
            PassleShortcodes = string.Join(", ", passleShortcodes);
        }
    }
}
