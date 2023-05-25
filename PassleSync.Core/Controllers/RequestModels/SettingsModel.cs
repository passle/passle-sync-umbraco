namespace PassleSync.Core.Controllers.RequestModels
{
    public class SettingsModel
    {
        public string PassleShortcodes { get; set; }
        public string ClientApiKey { get; set; }
        public string PluginApiKey { get; set; }
        public string PostPermalinkPrefix { get; set; }
        public string AuthorPermalinkPrefix { get; set; }
        public int PostsParentNodeId { get; set; }
        public int AuthorsParentNodeId { get; set; }
        public bool SimulateRemoteHosting { get; set; }
        public string PasslePermalinkPrefix { get; set; }
    }
}
