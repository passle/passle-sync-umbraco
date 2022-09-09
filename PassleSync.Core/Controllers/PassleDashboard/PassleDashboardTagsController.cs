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
using PassleSync.Core.API.Services;
using System;
using PassleSync.Core.Services;
using Umbraco.Core.Models.PublishedContent;
using PassleSync.Core.Constants;

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
        private readonly IPassleHelperService _passleHelperService;
        private readonly ConfigService _configService;

        public PassleDashboardTagsController(
            ITagService tagService, 
            PassleContentService<PassleTags, string> passleTagsContentService,
            PassleContentService<PasslePosts, PasslePost> passlePostsContentService,
            UmbracoContentService<PasslePost> umbracoPostsContentService,
            IPublishedContentQuery publishedContentQuery,
            IPassleHelperService passleHelperService,
            ConfigService configService)
        {
            _tagService = tagService;
            _passleTagsContentService = passleTagsContentService;
            _passlePostsContentService = passlePostsContentService;
            _umbracoPostsContentService = umbracoPostsContentService;
            _publishedContentQuery = publishedContentQuery;
            _passleHelperService = passleHelperService;
            _configService = configService;
        }

        [HttpGet]
        public IPassleDashboardViewModel GetAll()
        {
            var umbracoTags = _tagService.GetAllContentTags().Select(x => x.Text);
            IEnumerable<IPublishedContent> getUmbracoTaggedContent (string x) => _publishedContentQuery.Content(_tagService.GetTaggedContentByTag(x).Select(y => y.EntityId));
            int umbracoNonPassleContentCount(string x) => getUmbracoTaggedContent(x)
                .Where(y => y.ContentType.Alias != PassleContentType.PASSLE_POST)
                .Count();
            //int umbracoPassleContentCount(string x) => getUmbracoTaggedContent(x)
            //    .Where(y => y.ContentType.Alias == PassleContentType.PASSLE_POST)
            //    .Count();

            var passleTags = _passleTagsContentService.GetAll();
            var passlePosts = _passlePostsContentService.GetAll();

            //var syncedPasslePosts = _passleHelperService.GetPosts().Execute().Items;
            var syncedPasslePosts = _umbracoPostsContentService.GetContent();
            var syncedPasslePostTags = syncedPasslePosts.Select(x => x.Value<IEnumerable<string>>("tags"));
            var syncedPasslePostShortcodes = syncedPasslePosts.Select(x => x.Value<string>("postShortcode"));
            var unsyncedPasslePosts = passlePosts.Where(x => !syncedPasslePostShortcodes.Contains(x.PostShortcode));

            var allTags = umbracoTags.Union(passleTags)
                .Select(
                    x => new PassleDashboardTagViewModel()
                    {
                        Title = x,
                        NonPasslePostCount = umbracoNonPassleContentCount(x),
                        SyncedPasslePostCount = syncedPasslePosts.Count(y => syncedPasslePostTags.Any(z => z?.Contains(x) ?? false)),
                        //SyncedPasslePostCount = umbracoPassleContentCount(x),
                        UnsyncedPasslePostCount = unsyncedPasslePosts.Count(y => y.Tags.Contains(x))
                    }
                );

            var viewModel = new PassleDashboardTagsViewModel(allTags);

            return viewModel;
        }
    }
}
