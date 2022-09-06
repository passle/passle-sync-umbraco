using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Core.API.Services;
using PassleSync.Core.Models.Content.Umbraco;

namespace PassleSync.Website.Controllers
{
    public class PasslePostController : RenderMvcController
    {
        private readonly IPassleHelperService _passleHelperService;

        public PasslePostController(IPassleHelperService passleHelperService) : base()
        {
            _passleHelperService = passleHelperService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new PasslePost(model.Content);

            //var umbracoAuthors = Umbraco.Content(_configService.AuthorsParentNodeId)
            //    .ChildrenOfType(_configService.PassleAuthorContentTypeAlias)
            //    .Where(x => x.IsVisible())
            //    .Select(x => new PassleAuthorViewModel(x));

            //viewModel.UmbracoAuthorUrl = umbracoAuthors
            //    .Where(x => x.Shortcode == viewModel.Author.Shortcode)
            //    .FirstOrDefault()?.Url() ?? "";

            //ViewBag.PassleDomain = _configService.PassleDomain;
            //ViewBag.PassleSubscribeLink = "";

            return CurrentTemplate(viewModel);
        }
    }
}
