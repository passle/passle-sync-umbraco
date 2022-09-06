using PassleSync.Core.API.Services;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
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
            composition.Register<UmbracoContentService>(Lifetime.Request);
            composition.Register<IPassleHelperService, PassleHelperService>(Lifetime.Request);
        }
    }
}
