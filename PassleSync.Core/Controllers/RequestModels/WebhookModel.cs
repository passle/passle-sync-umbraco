using System.Collections.Generic;

namespace PassleSync.Core.Controllers.RequestModels
{
    public class WebhookModel
    {
        public WebhookAction Action;
        public Dictionary<string, string> Data;
    }
}
