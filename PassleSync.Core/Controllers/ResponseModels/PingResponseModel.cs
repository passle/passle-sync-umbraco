namespace PassleSync.Core.Controllers.ResponseModels
{
    public class PingResponseModel
    {
        public int RemoteHostingType = 1;
        public string PostPermalinkPrefix;
        public string PersonPermalinkPrefix;

        public PingResponseModel(string postPrefix, string authorPrefix)
        {
            PostPermalinkPrefix = postPrefix;
            PersonPermalinkPrefix = authorPrefix;
        }
    }
}
