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
using PassleSync.Core.Extensions;

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

            var umbracoPosts = _umbracoContentService.GetPosts();

            // Create viewmodels
            var umbracoPostModels = umbracoPosts.Select(post => new PassleDashboardPostViewModel(post));
            var apiPostModels = postsFromApi.Posts.Select(post => new PassleDashboardPostViewModel(post));

            var umbracoShortcodes = umbracoPostModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoPostModels.Concat(apiPostModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardPostsViewModel(allModels);
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

        public override bool SyncMany(string[] shortcodes)
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

            DeleteMany(shortcodes, postsParentNodeId);
            CreateMany(postsFromApi.Posts, postsParentNodeId, shortcodes);

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
            var posts = _umbracoContentService.GetPosts();

            foreach (var post in posts)
            {
                _contentService.Delete(post);
            }
        }

        public override bool DeleteMany(string[] shortcodes)
        {
            int postsParentNodeId = _configService.PostsParentNodeId;
            if (_contentService.GetById(postsParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(shortcodes, postsParentNodeId);
            return true;
        }

        public override void DeleteMany(string[] shortcodes, int parentNodeId)
        {
            var posts = _umbracoContentService.GetPosts().Where(x => shortcodes.Contains(x.GetValueOrDefault<string>("PostShortcode")));

            foreach (var post in posts)
            {
                _contentService.Delete(post);
            }
        }

        public override void CreateAll(IEnumerable<PasslePost> posts, int parentNodeId)
        {
            foreach (var post in posts)
            {
                CreateOne(post, parentNodeId);
            }
        }

        public override void CreateMany(IEnumerable<PasslePost> posts, int parentNodeId, string[] shortcodes)
        {
            foreach (var post in posts)
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

            node.CreateDate = post.PublishedDate;
            node.PublishDate = post.PublishedDate;
            node.UpdateDate = post.PublishedDate;

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

            var existingContent = _umbracoContentService.GetPostByShortcode(shortcode);
            if (existingContent == null)
            {
                CreateOne(postFromApi, postsParentNodeId);
            }
            else
            {
                var editableContent = _contentService.GetById(existingContent.Id);

                editableContent.Name = postFromApi.PostTitle;

                editableContent.CreateDate = postFromApi.PublishedDate;
                editableContent.PublishDate = postFromApi.PublishedDate;
                editableContent.UpdateDate = postFromApi.PublishedDate;

                AddAllPropertiesToNode(editableContent, postFromApi);

                _contentService.SaveAndPublish(editableContent, raiseEvents: false);
            }

            return true;
        }
    }
}
