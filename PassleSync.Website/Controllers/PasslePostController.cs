using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using Umbraco.Web;
using PassleSync.Core.Services;

namespace PassleSync.Website.Controllers
{
    public class PasslePostController : RenderMvcController
    {
        private readonly ConfigService _configService;

        public PasslePostController(ConfigService configService) : base()
        {
            _configService = configService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new PasslePostViewModel(model.Content);

            var umbracoAuthors = Umbraco.Content(_configService.AuthorsParentNodeId)
                .ChildrenOfType(_configService.PassleAuthorContentTypeAlias)
                .Where(x => x.IsVisible())
                .Select(x => new PassleAuthorViewModel(x));

            viewModel.UmbracoAuthorUrl = umbracoAuthors
                .Where(x => x.Shortcode == viewModel.Author.Shortcode)
                .FirstOrDefault()?.Url() ?? "";

            ViewBag.PassleDomain = _configService.PassleDomain;
            ViewBag.PassleSubscribeLink = "";

            return CurrentTemplate(viewModel);
        }
    }
}
