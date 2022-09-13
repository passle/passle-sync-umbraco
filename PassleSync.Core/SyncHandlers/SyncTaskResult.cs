using Umbraco.Core.Models;

namespace PassleSync.Core.SyncHandlers
{
    public class SyncTaskResult
    {
        public string Shortcode;
        public bool Success;
        public IContent Content;
    }
}
