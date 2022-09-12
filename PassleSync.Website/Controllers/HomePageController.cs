using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using PassleSync.Core.API.Services;
using PassleSync.Core.Extensions;
using System;

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
            var currentPage = Request.QueryString["page"].ToIntOrDefault(1);

            if (string.IsNullOrEmpty(searchQuery))
            {
                var viewModel = new HomePageViewModel(model.Content)
                {
                    Posts = _passleHelperService.GetPosts().FeaturedOnPasslePage(false).WithItemsPerPage(4).Execute().Items,
                    FeaturedPost = _passleHelperService.GetPosts().FeaturedOnPasslePage(true).Execute().Items.FirstOrDefault(),
                };

                return CurrentTemplate(viewModel);
            }
            else
            {
                var query = _passleHelperService.GetPosts().Search(searchQuery).WithCurrentPage(currentPage).WithItemsPerPage(10).Execute();

                var viewModel = new HomePageViewModel(model.Content)
                {
                    Posts = query.Items,
                    SearchQuery = searchQuery,
                    Pagination = new PaginationViewModel()
                    {
                        CurrentPage = query.CurrentPage,
                        TotalPages = query.TotalPages,
                    },
                };

                return CurrentTemplate(viewModel);
            }
        }
    }
}
