using System.Linq;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using System.Web.Http;
using PassleSync.Core.API.ViewModels;
using Umbraco.Core.Services;
using PassleSync.Core.Services.Content;
using System.Collections.Generic;
using PassleSync.Core.ViewModels.PassleDashboard;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Web;
using System;
using Umbraco.Core.Models.PublishedContent;
using PassleSync.Core.Constants;
using System.Threading.Tasks;

namespace PassleSync.Core.Controllers.PassleDashboard
{
    [PluginController("passleSync")]
    public class PassleDashboardTagsController : UmbracoAuthorizedJsonController
    {
        private readonly ITagService _tagService;
        private readonly PassleContentService<PassleTags, string> _passleTagsContentService;
        private readonly PassleContentService<PasslePosts, PasslePost> _passlePostsContentService;
        private readonly UmbracoContentService<PasslePost> _umbracoPostsContentService;
        private readonly IPublishedContentQuery _publishedContentQuery;

        public PassleDashboardTagsController(
            ITagService tagService, 
            PassleContentService<PassleTags, string> passleTagsContentService,
            PassleContentService<PasslePosts, PasslePost> passlePostsContentService,
            UmbracoContentService<PasslePost> umbracoPostsContentService,
            IPublishedContentQuery publishedContentQuery)
        {
            _tagService = tagService;
            _passleTagsContentService = passleTagsContentService;
            _passlePostsContentService = passlePostsContentService;
            _umbracoPostsContentService = umbracoPostsContentService;
            _publishedContentQuery = publishedContentQuery;
        }

        [HttpGet]
        public IPassleDashboardViewModel GetAll()
        {
            var umbracoTags = _tagService.GetAllContentTags().Select(x => x.Text);

            Func<string, int> umbracoNonPassleContentCount = x => _publishedContentQuery.Content(_tagService.GetTaggedContentByTag(x).Select(y => y.EntityId))
                .Count(y => y.ContentType.Alias != PassleContentType.PASSLE_POST);


            var passleTagsTask = Task.Run(() => _passleTagsContentService.GetAll());
            var passlePostsTask = Task.Run(() => _passlePostsContentService.GetAll());

            var passleTags = passleTagsTask.Result;
            var passlePosts = passlePostsTask.Result;


            var syncedPasslePosts = _umbracoPostsContentService.GetPublishedContent();
            var syncedPasslePostTags = syncedPasslePosts.Select(x => x.Value<IEnumerable<string>>("tags"));
            var syncedPasslePostShortcodes = syncedPasslePosts.Select(x => x.Value<string>("postShortcode"));
            var unsyncedPasslePosts = passlePosts.Where(x => !syncedPasslePostShortcodes.Contains(x.PostShortcode));

            var allTags = umbracoTags.Union(passleTags)
                .Select(
                    x => new PassleDashboardTagViewModel()
                    {
                        Title = x,
                        NonPasslePostCount = umbracoNonPassleContentCount(x),
                        SyncedPasslePostCount = syncedPasslePostTags.Count(y => y.Contains(x)),
                        UnsyncedPasslePostCount = unsyncedPasslePosts.Count(y => y.Tags.Contains(x))
                    }
                );

            var viewModel = new PassleDashboardTagsViewModel(allTags);

            return viewModel;
        }
    }
}
