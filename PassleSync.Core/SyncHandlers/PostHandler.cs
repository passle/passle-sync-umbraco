using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using PassleSync.Core.Helpers;
using PassleSync.Core.ViewModels.PassleDashboard;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using Umbraco.Core.Services;
using PassleSync.Core.Services;
using Umbraco.Core.Logging;
using PassleSync.Core.Constants;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : SyncHandlerBase<PasslePost>
    {
        public PostHandler(IContentService contentService, UmbracoContentService umbracoContentService, ConfigService configService, ILogger logger) : base(contentService, umbracoContentService, configService, logger)
        {
        }

        public override IPassleDashboardViewModel GetAll()
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardPostsViewModel(Enumerable.Empty<PassleDashboardPostViewModel>());
            }

            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
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

        private IEnumerable<IContent> GetAllUmbraco(int parentNodeId)
        {
            if (_contentService.HasChildren(parentNodeId))
            {
                return _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
        }

        public override bool SyncAll()
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            DeleteAll(postsParentNodeId);
            CreateAll(postsFromApi.Posts, postsParentNodeId);

            return true;
        }

        public override bool SyncMany(string[] Shortcodes)
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(Shortcodes, postsParentNodeId);
            CreateMany(postsFromApi.Posts, postsParentNodeId, Shortcodes);

            return true;
        }

        public override bool DeleteAll()
        {
            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            DeleteAll(postsParentNodeId);
            return true;
        }

        public override void DeleteAll(int parentNodeId)
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

        public override bool DeleteMany(string[] Shortcodes)
        {
            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(Shortcodes, postsParentNodeId);
            return true;
        }

        public override void DeleteMany(string[] shortcodes, int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (shortcodes.Contains(child.GetValue<string>("postShortcode")))
                    {
                        _contentService.Delete(child);
                    }
                }
            }
        }

        private void DeleteOne(string shortcode, int parentNodeId)
        {
            DeleteMany(new string[] { shortcode }, parentNodeId);
        }

        public override void CreateAll(IEnumerable<PasslePost> posts, int parentNodeId)
        {
            foreach (PasslePost post in posts)
            {
                CreateOne(post, parentNodeId);
            }
        }

        public override void CreateMany(IEnumerable<PasslePost> posts, int parentNodeId, string[] shortcodes)
        {
            foreach (PasslePost post in posts)
            {
                if (shortcodes.Contains(post.PostShortcode))
                {
                    CreateOne(post, parentNodeId);
                }
            }
        }

        public override void CreateOne(PasslePost post, int parentNodeId)
        {
            var node = _contentService.Create(post.PostTitle, parentNodeId, PassleContentType.PASSLE_POST);

            var date = DateTime.Parse(post.PublishedDate);
            node.CreateDate = date;
            node.PublishDate = date;
            node.UpdateDate = date;

            AddAllPropertiesToNode(node, post);

            _contentService.SaveAndPublish(node);
        }

        public override bool SyncOne(string shortcode)
        {
            var postsFromApi = ApiHelper.GetPosts();
            if (postsFromApi == null || postsFromApi.Posts == null)
            {
                // Failed to get posts from the API
                return false;
            }

            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            var postFromApi = postsFromApi.Posts.FirstOrDefault(x => x.PostShortcode == shortcode);
            if (postFromApi == null)
            {
                return false;
            }

            var publishedContent = _umbracoContentService.GetPublishedPostByShortcode(shortcode);
            if (publishedContent == null)
            {
                CreateOne(postFromApi, postsParentNodeId);
            }
            else
            {
                var editableContent = _contentService.GetById(publishedContent.Id);

                editableContent.Name = postFromApi.PostTitle;

                var date = DateTime.Parse(postFromApi.PublishedDate);
                editableContent.CreateDate = date;
                editableContent.PublishDate = date;
                editableContent.UpdateDate = date;

                AddAllPropertiesToNode(editableContent, postFromApi);

                _contentService.SaveAndPublish(editableContent, raiseEvents: false);
            }

            return true;
        }
    }
}
