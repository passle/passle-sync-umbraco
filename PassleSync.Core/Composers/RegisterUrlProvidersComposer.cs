using PassleSync.Core.UrlProviders;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace PassleSync.Core.Composers
{
    public class RegisterUrlProvidersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.UrlProviders().Insert<PasslePostUrlProvider>();
            composition.UrlProviders().Insert<PassleAuthorUrlProvider>();
        }
    }
}
