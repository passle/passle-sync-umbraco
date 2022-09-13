using PassleSync.Core.API.Models.Conntent.PassleApi;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PassleTag : IPassleApiResponseModel
    {
        public string Tag { get; set; }
        public string GetShortcode()
        {
            return Tag;
        }
    }
}
