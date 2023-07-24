namespace PassleSync.Core.Controllers.RequestModels
{
    public class SettingsModel
    {
        public string PassleShortcodes { get; set; }
        public string ClientApiKey { get; set; }
        public string PluginApiKey { get; set; }
        public string PostPermalinkTemplate { get; set; }
        public string PersonPermalinkTemplate { get; set; }
        public string PreviewPermalinkTemplate { get; set; }
        public bool SimulateRemoteHosting { get; set; }
        public int PostsParentNodeId { get; set; }
        public int AuthorsParentNodeId { get; set; }
    }
}
