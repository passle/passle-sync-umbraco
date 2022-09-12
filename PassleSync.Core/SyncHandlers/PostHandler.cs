using System;
using System.Linq;
using PassleSync.Core.ViewModels.PassleDashboard;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Core.Services;
using PassleSync.Core.Services;
using Umbraco.Core.Logging;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : SyncHandlerBase<PasslePosts, PasslePost>
    {
        public PostHandler(
            IContentService contentService,
            ConfigService configService,
            PassleContentService<PasslePosts, PasslePost> passleContentService,
            UmbracoContentService<PasslePost> umbracoContentService,
            ILogger logger
        ) : base(
            contentService,
            configService,
            passleContentService,
            umbracoContentService,
            logger
        )
        {
        }

        public override IPassleDashboardViewModel GetAll()
        {
            var postsFromApi = _passleContentService.GetAll();
            if (postsFromApi == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }

            var umbracoPosts = _umbracoContentService.GetPublishedContent();

            // Create viewmodels
            var umbracoPostModels = umbracoPosts.Select(post => new PassleDashboardPostViewModel(post));
            var apiPostModels = postsFromApi.Select(post => new PassleDashboardPostViewModel(post));

            var umbracoShortcodes = umbracoPostModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoPostModels.Concat(apiPostModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardPostsViewModel(allModels);
        }

        public override string Shortcode(PasslePost item)
        {
            return item.PostShortcode;
        }
    }
}
