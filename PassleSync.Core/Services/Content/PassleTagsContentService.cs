using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;

namespace PassleSync.Core.Services.Content
{
    public class PassleTagsContentService : PassleContentService<PassleTags, PassleTag>
    {
        public PassleTagsContentService(
            ApiService apiService,
            ConfigService configService) : base (apiService, configService)
        {
            _path = "api/v2/tags/" + _configService.PassleShortcodesString;
            _responseSelector = x => x.Tags;
        }
    }
}
