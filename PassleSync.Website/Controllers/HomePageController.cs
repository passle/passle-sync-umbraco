using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using PassleSync.Core.API.Services;
using PassleSync.Core.Models.Content.Umbraco;
using PassleSync.Website.Extensions;

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
            var searchQuery = Request.QueryString["s"];
            var tagFilter = Request.QueryString["tag"];
            var currentPage = int.Parse(Request.QueryString.GetValueOrDefault("page", "1"));

            var query = _passleHelperService.GetPosts().WithItemsPerPage(4);
            PasslePost featuredPost = null;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Search(searchQuery).WithCurrentPage(currentPage).WithItemsPerPage(10);
            }
            else if (!string.IsNullOrEmpty(tagFilter))
            {
                query = query.ByTag(tagFilter).WithCurrentPage(currentPage).WithItemsPerPage(10);
            }
            else
            {
                query = query.FeaturedOnPasslePage(false).WithItemsPerPage(4);
                featuredPost = _passleHelperService.GetPosts().FeaturedOnPasslePage(true).Execute().Items.FirstOrDefault();
            }

            var queryResult = query.Execute();

            var viewModel = new HomePageViewModel(model.Content)
            {
                Posts = queryResult.Items,
                FeaturedPost = featuredPost,
                SearchQuery = searchQuery,
                TagFilter = tagFilter,
                Pagination = new PaginationViewModel()
                {
                    CurrentPage = queryResult.CurrentPage,
                    TotalPages = queryResult.TotalPages,
                },
            };

            return CurrentTemplate(viewModel);
        }
    }
}
