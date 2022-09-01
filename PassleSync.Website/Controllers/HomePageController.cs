using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using Umbraco.Web;
using PassleSync.Core.Services;
using PassleSync.Core.Constants;

namespace PassleSync.Website.Controllers
{
    public class HomePageController : RenderMvcController
    {
        private readonly ConfigService _configService;

        public HomePageController(ConfigService configService) : base()
        {
            _configService = configService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new HomePageViewModel(model.Content);

            var posts = Umbraco.Content(_configService.PostsParentNodeId)
                .ChildrenOfType(PassleContentType.PASSLE_POST)
                .Where(x => x.IsVisible())
                .Select(x => new PasslePostViewModel(x));

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
