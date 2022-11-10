using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;

namespace PassleSync.Core.Services.Content
{
    public class PassleAuthorsContentService : PassleContentService<PassleAuthors, PassleAuthor>
    {
        public PassleAuthorsContentService(
            ApiService apiService,
            ConfigService configService) : base (apiService, configService)
        {
            _path = "api/v2/passlesync/people";
            _responseSelector = x => x.People;
            _itemType = "PersonShortcode";
        }
    }
}
