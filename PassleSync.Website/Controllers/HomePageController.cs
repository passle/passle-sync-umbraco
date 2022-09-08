using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using PassleSync.Core.API.Services;

namespace PassleSync.Website.Controllers
{
    public class HomePageController : RenderMvcController
    {
        private readonly IPassleHelperService _passleHelperService;

        public HomePageController(IPassleHelperService passleHelperService) : base()
        {
            _passleHelperService = passleHelperService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new HomePageViewModel(model.Content)
            {
                Posts = _passleHelperService.GetPosts().FeaturedOnPasslePage(false).WithItemsPerPage(4).Execute().Items,
                FeaturedPost = _passleHelperService.GetPosts().FeaturedOnPasslePage(true).Execute().Items.FirstOrDefault(),
            };

            return CurrentTemplate(viewModel);
        }
    }
}
