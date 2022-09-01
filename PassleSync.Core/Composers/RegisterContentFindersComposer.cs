using PassleSync.Core.ContentFinders;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace PassleSync.Core.Composers
{
    public class RegisterContentFindersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.ContentFinders().Insert<PasslePostContentFinder>();
            composition.ContentFinders().Insert<PassleAuthorContentFinder>();
        }
    }
}
