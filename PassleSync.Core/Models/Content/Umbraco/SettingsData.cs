using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public class SettingsData
    {
        public IEnumerable<string> PassleShortcodes { get; set; }
        public string ClientApiKey { get; set; }
        public string PluginApiKey { get; set; }
        public string PostPermalinkPrefix { get; set; }
        public string AuthorPermalinkPrefix { get; set; }
        public int PostsParentNodeId { get; set; }
        public int AuthorsParentNodeId { get; set; }
        public bool SimulateRemoteHosting { get; set; }
        public bool UseHttps { get; set; }
        public string CustomDomain { get; set; }
        public string PasslePermalinkPrefix { get; set; }
    }
}
