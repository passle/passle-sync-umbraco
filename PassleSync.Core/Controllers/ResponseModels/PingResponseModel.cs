using PassleSync.Core.Constants;

namespace PassleSync.Core.Controllers.ResponseModels
{
    public class PingResponseModel
    {
        public int RemoteHostingType = PassleRemoteHostingType.UMBRACO;
        public string PostPermalinkTemplate;
        public string PersonPermalinkTemplate;

        public PingResponseModel(string postTemplate, string personTemplate)
        {
            PostPermalinkTemplate = postTemplate;
            PersonPermalinkTemplate = personTemplate;
        }
    }
}
