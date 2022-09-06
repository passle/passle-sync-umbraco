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
            var viewModel = new HomePageViewModel(model.Content);

            var posts = _passleHelperService.GetPosts();
            var featuredPost = posts.Where(x => x.IsFeaturedOnPasslePage).FirstOrDefault();

            if (featuredPost != null)
            {
                viewModel.FeaturedPost = featuredPost;
                viewModel.Posts = posts.Where(x => x.PostShortcode != featuredPost.PostShortcode);
            }
            else
            {
                viewModel.Posts = posts;
            }

            return CurrentTemplate(viewModel);
        }
    }
}
