using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : ISyncHandler<Post>
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService { get; set; }


        public PostHandler(IKeyValueService keyValueService, IContentService contentService)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
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
                return false;
            }

            DeleteAll(postsParentNodeId);
            CreateMany(postsFromApi.Posts, postsParentNodeId);

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
            } catch (ArgumentNullException ex)
            {
                return false;
            }

            DeleteMany(Shortcodes, postsParentNodeId);
            CreateMany(postsFromApi.Posts, postsParentNodeId);

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

        public void CreateMany(IEnumerable<Post> posts, int parentNodeId)
        {
            foreach (Post post in posts)
            {
                CreateOne(post, parentNodeId);
            }
        }

        public void CreateOne(Post post, int parentNodeId)
        {
            // TODO: Const for "post"
            var node = _contentService.Create(post.PostTitle, parentNodeId, "post");

            // TODO: Should these strings be consts?
            // TODO: Capitalisation?
            node.SetValue("PostContentHtml", post.PostContentHtml);
            node.SetValue("FeaturedItemHtml", post.FeaturedItemHtml);
            node.SetValue("FeaturedItemPosition", post.FeaturedItemPosition);
            node.SetValue("QuoteText", post.QuoteText);
            node.SetValue("QuoteUrl", post.QuoteUrl);
            node.SetValue("Tweets", post.Tweets);
            node.SetValue("IsFeaturedOnPasslePage", post.IsFeaturedOnPasslePage);
            node.SetValue("IsFeaturedOnPostPage", post.IsFeaturedOnPostPage);
            node.SetValue("PostShortcode", post.PostShortcode);
            node.SetValue("PassleShortcode", post.PassleShortcode);
            node.SetValue("PostUrl", post.PostUrl);
            node.SetValue("PostTitle", post.PostTitle);
            node.SetValue("Authors", post.Authors);
            node.SetValue("CoAuthors", post.CoAuthors);
            node.SetValue("ShareViews", post.ShareViews);
            node.SetValue("ContentTextSnippet", post.ContentTextSnippet);
            node.SetValue("PublishedDate", post.PublishedDate);
            node.SetValue("Tags", post.Tags);
            node.SetValue("FeaturedItemMediaType", post.FeaturedItemMediaType);
            node.SetValue("FeaturedItemEmbedType", post.FeaturedItemEmbedType);
            node.SetValue("FeaturedItemEmbedProvider", post.FeaturedItemEmbedProvider);
            node.SetValue("ImageUrl", post.ImageUrl);
            node.SetValue("TotalShares", post.TotalShares);
            node.SetValue("IsRepost", post.IsRepost);
            node.SetValue("EstimatedReadTimeInSeconds", post.EstimatedReadTimeInSeconds);
            node.SetValue("TotalLikes", post.TotalLikes);
            node.SetValue("OpensInNewTab", post.OpensInNewTab);

            _contentService.SaveAndPublish(node);
        }

        public bool SyncOne(string Shortcode)
        {
            var postsFromApi = ApiHelper.GetPosts();

            // TODO: Move this into a config service?
            var postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));
            var postsParentNode = _contentService.GetById(postsParentNodeId);

            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(postsParentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(postsParentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("PostShortcode") == Shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            // Create a new post
            var postfromApi = postsFromApi.Posts.FirstOrDefault(x => x.PostShortcode == Shortcode);
            if (postfromApi != null)
            {
                // TODO: Const for "post"
                var node = _contentService.Create(postfromApi.PostTitle, postsParentNode.Id, "post");

                // TODO: Should these strings be consts?
                // TODO: Capitalisation?
                node.SetValue("PostContentHtml", postfromApi.PostContentHtml);
                node.SetValue("FeaturedItemHtml", postfromApi.FeaturedItemHtml);
                node.SetValue("FeaturedItemPosition", postfromApi.FeaturedItemPosition);
                node.SetValue("QuoteText", postfromApi.QuoteText);
                node.SetValue("QuoteUrl", postfromApi.QuoteUrl);
                node.SetValue("Tweets", postfromApi.Tweets);
                node.SetValue("IsFeaturedOnPasslePage", postfromApi.IsFeaturedOnPasslePage);
                node.SetValue("IsFeaturedOnPostPage", postfromApi.IsFeaturedOnPostPage);
                node.SetValue("PostShortcode", postfromApi.PostShortcode);
                node.SetValue("PassleShortcode", postfromApi.PassleShortcode);
                node.SetValue("PostUrl", postfromApi.PostUrl);
                node.SetValue("PostTitle", postfromApi.PostTitle);
                node.SetValue("Authors", postfromApi.Authors);
                node.SetValue("CoAuthors", postfromApi.CoAuthors);
                node.SetValue("ShareViews", postfromApi.ShareViews);
                node.SetValue("ContentTextSnippet", postfromApi.ContentTextSnippet);
                node.SetValue("PublishedDate", postfromApi.PublishedDate);
                node.SetValue("Tags", postfromApi.Tags);
                node.SetValue("FeaturedItemMediaType", postfromApi.FeaturedItemMediaType);
                node.SetValue("FeaturedItemEmbedType", postfromApi.FeaturedItemEmbedType);
                node.SetValue("FeaturedItemEmbedProvider", postfromApi.FeaturedItemEmbedProvider);
                node.SetValue("ImageUrl", postfromApi.ImageUrl);
                node.SetValue("TotalShares", postfromApi.TotalShares);
                node.SetValue("IsRepost", postfromApi.IsRepost);
                node.SetValue("EstimatedReadTimeInSeconds", postfromApi.EstimatedReadTimeInSeconds);
                node.SetValue("TotalLikes", postfromApi.TotalLikes);
                node.SetValue("OpensInNewTab", postfromApi.OpensInNewTab);

                _contentService.SaveAndPublish(node);

                return true;
            }
            else
            {
                // Clearly we've not managed to delete all the existing posts if one still exists
                return false;
            }
        }
    }
}
