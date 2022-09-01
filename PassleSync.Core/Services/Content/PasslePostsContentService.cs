using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;

namespace PassleSync.Core.Services.Content
{
    public class PasslePostsContentService : PassleContentService<PasslePosts, PasslePost>
    {
        public PasslePostsContentService(
            ApiService apiService,
            ConfigService configService) : base (apiService, configService)
        {
            _path = "api/v2/passlesync/posts";
            _responseSelector = x => x.Posts;
        }
    }
}
