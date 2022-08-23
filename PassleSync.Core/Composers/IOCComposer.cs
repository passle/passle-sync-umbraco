using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;
using PassleSync.Core.SyncHandlers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Composers
{
    public class IOCComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<ISyncHandler<Post>, PostHandler>(Lifetime.Request);
            composition.Register<ISyncHandler<Person>, AuthorHandler>(Lifetime.Request);
        }
    }
}
