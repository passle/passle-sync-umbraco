using PassleSync.Core.Controllers.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassleSync.Core.Models
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
    }
}
