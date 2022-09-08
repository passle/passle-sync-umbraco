using PassleSync.Core.Constants;
using PassleSync.Core.Controllers.RequestModels;
using PassleSync.Core.Extensions;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace PassleSync.Core.Controllers.Webhooks
{
    [PluginController("Passle")]
    public class FeaturedPostController : UmbracoApiController
    {
        private readonly IContentService _contentService;

        public FeaturedPostController()
        {
            _contentService = Services.ContentService;
        }

        public IHttpActionResult Update(FeaturedPostModel model)
        {
            var virtualContentType = UmbracoContext.Content.GetContentType(PassleContentType.PASSLE_POST);
            var virtualContent = UmbracoContext.Content.GetByContentType(virtualContentType);

            var publishedContent = virtualContent.FirstOrDefault(x => x.GetValueOrDefault<string>("PostShortcode") == model.PostShortcode);
            if (publishedContent == null)
            {
                return BadRequest();
            }

            // Clear featured posts
            // TODO: It feels like there is probably a better way of doing this
            var publishedFeaturedContent = virtualContent.Where(x => x.GetValueOrDefault<string>("IsFeaturedOnPasslePage") == "True" || x.GetValueOrDefault<string>("IsFeaturedOnPostPage") == "True");
            foreach (var contentItem in publishedFeaturedContent)
            {
                var editableContentItem = _contentService.GetById(contentItem.Id);
                
                editableContentItem.SetValue("IsFeaturedOnPasslePage", false);
                editableContentItem.SetValue("IsFeaturedOnPostPage", false);

                _contentService.SaveAndPublish(editableContentItem, raiseEvents: false);
            }

            // Set the new post as featured on the Passle page/post page
            var editableContent = _contentService.GetById(publishedContent.Id);

            editableContent.SetValue("IsFeaturedOnPasslePage", model.IsFeaturedOnPasslePage);
            editableContent.SetValue("IsFeaturedOnPostPage", model.IsFeaturedOnPostPage);

            _contentService.SaveAndPublish(editableContent, raiseEvents: false);

            return Ok();
        }
    }
}
