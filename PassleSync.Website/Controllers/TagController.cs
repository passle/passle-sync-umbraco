using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Website.ViewModels;
using Umbraco.Web;
using Umbraco.Core.Services;
using System;

namespace PassleSync.Website.Controllers
{
    public class TagController : RenderMvcController
    {
        private readonly ITagService _tagService;
        private readonly IPublishedContentQuery _publishedContentQuery;

        public TagController(ITagService tagService, IPublishedContentQuery publishedContentQuery) : base()
        {
            _publishedContentQuery = publishedContentQuery;
            _tagService = tagService;
        }

        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new TagViewModel(model.Content);

            try
            {
                var tagName = Request.Params["name"];

                viewModel.Tag = tagName;

                var taggedPosts = _tagService.GetTaggedContentByTag(viewModel.Tag);
                viewModel.Posts = _publishedContentQuery.Content(taggedPosts.Select(x => x.EntityId))
                    .Select(x => new PasslePostViewModel(x));

                return CurrentTemplate(viewModel);
            }
            catch (Exception)
            {
                viewModel.Tag = "";
                viewModel.Posts = Enumerable.Empty<PasslePostViewModel>();

                return CurrentTemplate(viewModel);
            }
        }
    }
}
