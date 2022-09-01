using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using Umbraco.Web;
using PassleSync.Core.Services;

namespace PassleSync.Website.Controllers
{
    public class PassleAuthorController : RenderMvcController
    {
        private readonly ConfigService _configService;

        public PassleAuthorController(ConfigService configService) : base()
        {
            _configService = configService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new PassleAuthorViewModel(model.Content);

            var umbracoPosts = Umbraco.Content(_configService.PostsParentNodeId)
                .ChildrenOfType(_configService.PasslePostContentTypeAlias)
                .Where(x => x.IsVisible())
                .Select(x => new PasslePostViewModel(x));

            viewModel.AuthorPosts = umbracoPosts
                .Where(x => x.Authors.Select(y => y.PassleName)
                .Contains(viewModel.PassleName));

            return CurrentTemplate(viewModel);
        }
    }
}
