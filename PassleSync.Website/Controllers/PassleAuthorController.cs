using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Core.API.Services;
using PassleSync.Core.Models.Content.Umbraco;

namespace PassleSync.Website.Controllers
{
    public class PassleAuthorController : RenderMvcController
    {
        private readonly IPassleHelperService _passleHelperService;

        public PassleAuthorController(IPassleHelperService passleHelperService) : base()
        {
            _passleHelperService = passleHelperService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new PassleAuthor(model.Content);

            //var umbracoPosts = Umbraco.Content(_configService.PostsParentNodeId)
            //    .ChildrenOfType(PassleContentType.PASSLE_POST)
            //    .Where(x => x.IsVisible())
            //    .Select(x => new PasslePostViewModel(x));

            //viewModel.AuthorPosts = umbracoPosts
            //    .Where(x => x.Authors.Select(y => y.PassleName)
            //    .Contains(viewModel.PassleName));

            return CurrentTemplate(viewModel);
        }
    }
}
