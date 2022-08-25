using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : ISyncHandler<PasslePost>
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService;
        protected readonly ILogger _logger;


        public PostHandler(
            IKeyValueService keyValueService,
            IContentService contentService,
            ILogger logger)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
            _logger = logger;
        }

        public IPassleDashboardViewModel GetAll()
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }

            // TODO: Move this into a config service?
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }

            var umbracoPosts = GetAllUmbraco(postsParentNodeId);

            // Create viewmodels
            var umbracoPostModels = umbracoPosts.Select(post => new PassleDashboardPostViewModel(post));
            var apiPostModels = postsFromApi.Posts.Select(post => new PassleDashboardPostViewModel(post));

            var umbracoShortcodes = umbracoPostModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoPostModels.Concat(apiPostModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardPostsViewModel(allModels);
        }

        public IEnumerable<IContent> GetAllUmbraco(int parentNodeId)
        {
            if (_contentService.HasChildren(parentNodeId))
            {
                return _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
        }

        public bool SyncAll()
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            // TODO: Move this into a config service?
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteAll(postsParentNodeId);
            CreateAll(postsFromApi.Posts, postsParentNodeId);

            return true;
        }

        public bool SyncMany(string[] Shortcodes)
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            // TODO: Move this into a config service?
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteMany(Shortcodes, postsParentNodeId);
            CreateMany(postsFromApi.Posts, postsParentNodeId, Shortcodes);

            return true;
        }

        public bool DeleteAll()
        {
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteAll(postsParentNodeId);
            return true;
        }

        public void DeleteAll(int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    _contentService.Delete(child);
                }
            }
        }

        public bool DeleteMany(string[] Shortcodes)
        {
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteMany(Shortcodes, postsParentNodeId);
            return true;
        }

        public void DeleteMany(string[] Shortcodes, int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (Shortcodes.Contains(child.GetValue<string>("postShortcode")))
                    {
                        _contentService.Delete(child);
                    }
                }
            }
        }

        public void DeleteOne(string Shortcode, int parentNodeId)
        {
            DeleteMany(new string[] { Shortcode }, parentNodeId);
        }

        public void CreateAll(IEnumerable<PasslePost> posts, int parentNodeId)
        {
            foreach (PasslePost post in posts)
            {
                CreateOne(post, parentNodeId);
            }
        }

        public void CreateMany(IEnumerable<PasslePost> posts, int parentNodeId, string[] Shortcodes)
        {
            foreach (PasslePost post in posts)
            {
                if (Shortcodes.Contains(post.PostShortcode))
                {
                    CreateOne(post, parentNodeId);
                }
            }
        }

        public void CreateOne(PasslePost post, int parentNodeId)
        {
            var node = _contentService.Create(post.PostTitle, parentNodeId, _configService.PasslePostContentTypeAlias);

            node.SetValue(PasslePost.PostTitleProperty, post.PostTitle);
            node.SetValue(PasslePost.PostShortcodeProperty, post.PostShortcode);
            node.SetValue(PasslePost.PassleShortcodeProperty, post.PassleShortcode);
            node.SetValue(PasslePost.PublishedDateProperty, post.PublishedDate);
            node.SetValue(PasslePost.PostUrlProperty, post.PostUrl);
            node.SetValue(PasslePost.ImageUrlProperty, post.ImageUrl);
            node.SetValue(PasslePost.AuthorsProperty, JsonSerializer.Serialize(post.Authors));
            node.SetValue(PasslePost.CoAuthorsProperty, JsonSerializer.Serialize(post.CoAuthors));
            node.SetValue(PasslePost.PostContentHtmlProperty, post.PostContentHtml);
            node.SetValue(PasslePost.ContentTextSnippetProperty, post.ContentTextSnippet);
            node.SetValue(PasslePost.QuoteTextProperty, post.QuoteText);
            node.SetValue(PasslePost.QuoteUrlProperty, post.QuoteUrl);
            node.SetValue(PasslePost.IsRepostProperty, post.IsRepost);
            node.SetValue(PasslePost.IsFeaturedOnPasslePageProperty, post.IsFeaturedOnPasslePage);
            node.SetValue(PasslePost.IsFeaturedOnPostPageProperty, post.IsFeaturedOnPostPage);
            node.SetValue(PasslePost.EstimatedReadTimeInSecondsProperty, post.EstimatedReadTimeInSeconds);
            node.SetValue(PasslePost.FeaturedItemHtmlProperty, post.FeaturedItemHtml);
            node.SetValue(PasslePost.FeaturedItemPositionProperty, post.FeaturedItemPosition);
            node.SetValue(PasslePost.FeaturedItemMediaTypeProperty, post.FeaturedItemMediaType);
            node.SetValue(PasslePost.FeaturedItemEmbedTypeProperty, post.FeaturedItemEmbedType);
            node.SetValue(PasslePost.FeaturedItemEmbedProviderProperty, post.FeaturedItemEmbedProvider);
            node.SetValue(PasslePost.TagsProperty, string.Join(",", post.Tags));
            node.SetValue(PasslePost.TweetsProperty, JsonSerializer.Serialize(post.Tweets));
            node.SetValue(PasslePost.ShareViewsProperty, JsonSerializer.Serialize(post.ShareViews));
            node.SetValue(PasslePost.TotalSharesProperty, post.TotalShares);
            node.SetValue(PasslePost.TotalLikesProperty, post.TotalLikes);
            node.SetValue(PasslePost.OpensInNewTabProperty, post.OpensInNewTab);

            _contentService.SaveAndPublish(node);
        }

        public bool SyncOne(string Shortcode)
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            // TODO: Move this into a config service?
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            var postFromApi = postsFromApi.Posts.FirstOrDefault(x => x.PostShortcode == Shortcode);
            if (postFromApi == null)
            {
                return false;
            }

            DeleteOne(Shortcode, postsParentNodeId);
            CreateOne(postFromApi, postsParentNodeId);

            return true;
        }
    }
}
