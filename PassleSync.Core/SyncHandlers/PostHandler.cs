using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using PassleSync.Core.Helpers;
using PassleSync.Core.ViewModels.PassleDashboard;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Extensions;
using System.Collections;
using Umbraco.Core;
using Umbraco.Core.Services;
using PassleSync.Core.Services;
using Umbraco.Core.Logging;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : SyncHandlerBase<PasslePost>
    {
        public PostHandler(IContentService contentService, ConfigService configService, ILogger logger) : base(contentService, configService, logger)
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

        public override void CreateMany(IEnumerable<PasslePost> posts, int parentNodeId, string[] Shortcodes)
        {
            foreach (PasslePost post in posts)
            {
                if (Shortcodes.Contains(post.PostShortcode))
                {
                    CreateOne(post, parentNodeId);
                }
            }
        }

        public override void CreateOne(PasslePost post, int parentNodeId)
        {
            var node = _contentService.Create(post.PostTitle, parentNodeId, _configService.PasslePostContentTypeAlias);

            var properties = post.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyTypeInfo = property.PropertyType;

                if (propertyTypeInfo.Implements<IEnumerable>() && propertyTypeInfo.IsGenericType)
                {
                    propertyTypeInfo = propertyTypeInfo.GetGenericArguments()[0];
                }
                else if (!propertyTypeInfo.IsSimpleType())
                {
                    continue;
                }

                if (propertyTypeInfo.IsSerializable)
                {
                    AddPropertyToNode(node, post, property.Name);
                }
                else
                {
                    AddNestedContentToNode(node, post, propertyTypeInfo, property.Name);
                }
            }

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

            DeleteOne(shortcode, postsParentNodeId);
            CreateOne(postFromApi, postsParentNodeId);

            return true;
        }
    }
}
