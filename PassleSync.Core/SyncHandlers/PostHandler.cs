using System;
using System.Linq;
using PassleSync.Core.ViewModels.PassleDashboard;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Core.Services;
using PassleSync.Core.Services;
using Umbraco.Core.Logging;
using PassleSync.Core.Services.Content;
using PassleSync.Core.Extensions;
using Umbraco.Core.Models.PublishedContent;
using System.Collections.Generic;
using System.Net.Http;
using PassleSync.Core.Exceptions;

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
            IEnumerable<PasslePost> postsFromApi;
            try
            {
                postsFromApi = _passleContentService.GetAll();
            }
            catch (PassleException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new PassleException(typeof(PasslePost), PassleExceptionEnum.UNKNOWN);
            }

            if (postsFromApi == null)
            {
                // Failed to get posts from the API
                throw new PassleException(typeof(PasslePost), PassleExceptionEnum.NULL_FROM_API);
            }

            var umbracoPosts = _umbracoContentService.GetAllContent();

            // Create viewmodels
            var umbracoPostModels = umbracoPosts.Select(post => new PassleDashboardPostViewModel(post));
            var apiPostModels = postsFromApi.Select(post => new PassleDashboardPostViewModel(post));

            var umbracoShortcodes = umbracoPostModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoPostModels.Concat(apiPostModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardPostsViewModel(allModels);
        }

        public override IPassleDashboardViewModel GetExisting()
        {
            var umbracoPosts = _umbracoContentService.GetAllContent();
            var umbracoPostModels = umbracoPosts.Select(post => new PassleDashboardPostViewModel(post));
            return new PassleDashboardPostsViewModel(umbracoPostModels);
        }

        public override string Shortcode(PasslePost item)
        {
            return item.PostShortcode;
        }
    }
}
