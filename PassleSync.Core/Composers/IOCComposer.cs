using PassleSync.Core.API.Services;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Helpers.Queries;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.API;
using PassleSync.Core.Services.Content;
using PassleSync.Core.SyncHandlers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Composers
{
    public class IOCComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<ISyncHandler<PasslePost>, PostHandler>(Lifetime.Request);
            composition.Register<ISyncHandler<PassleAuthor>, AuthorHandler>(Lifetime.Request);
            composition.Register<ConfigService>(Lifetime.Request);
            composition.Register<ApiService>(Lifetime.Request);
            composition.Register<PassleContentService<PasslePosts, PasslePost>, PasslePostsContentService>(Lifetime.Request);
            composition.Register<PassleContentService<PassleAuthors, PassleAuthor>, PassleAuthorsContentService>(Lifetime.Request);
            composition.Register<PassleContentService<PassleTags, PassleTag>, PassleTagsContentService>(Lifetime.Request);
            composition.Register<UmbracoContentService<PasslePost>, UmbracoPostsContentService>(Lifetime.Request);
            composition.Register<UmbracoContentService<PassleAuthor>, UmbracoAuthorsContentService>(Lifetime.Request);
            composition.Register<IPassleHelperService, PassleHelperService>(Lifetime.Request);
            composition.Register<PasslePostQuery>(Lifetime.Request);
            composition.Register<PassleAuthorQuery>(Lifetime.Request);
        }
    }
}
