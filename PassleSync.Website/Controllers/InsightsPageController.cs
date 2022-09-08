using PassleSync.Core.API.Services;
using PassleSync.Website.ViewModels;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PassleSync.Website.Controllers
{
    public class InsightsPageController : RenderMvcController
    {
        private readonly IPassleHelperService _passleHelperService;

        public InsightsPageController(IPassleHelperService passleHelperService) : base()
        {
            _passleHelperService = passleHelperService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var currentPage = int.Parse(Request.QueryString["page"]);
            var query = _passleHelperService.GetPosts().WithCurrentPage(currentPage).WithItemsPerPage(10).Execute();

            var viewModel = new InsightsPageViewModel(model.Content)
            {
                Posts = query.Items,
                Pagination = new PaginationViewModel()
                {
                    CurrentPage = query.CurrentPage,
                    TotalPages = query.TotalItems / query.ItemsPerPage,
                }
            };

            return CurrentTemplate(viewModel);
        }
    }
}
