using PassleSync.Core.API.Actions;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.Actions
{
    public class UpdateFeaturedPostAction : IActionBase<UpdateFeaturedPostActionModel>
    {
        protected readonly UmbracoContentService<PasslePost> _umbracoContentService;

        public UpdateFeaturedPostAction(UmbracoContentService<PasslePost> umbracoContentService)
        {
            _umbracoContentService = umbracoContentService;
        }
        public void Execute(UpdateFeaturedPostActionModel model)
        {
            _umbracoContentService.ClearFeaturedContent();
            _umbracoContentService.SetFeaturedContent(model.Shortcode, model.IsFeaturedOnPasslePage, model.IsFeaturedOnPostPage);
        }
    }
}
