using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.API.SyncHandlers;

namespace PassleSync.Core.SyncHandlers
{
    public class PostHandler : ISyncHandler
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService { get; set; }


        public PostHandler(IKeyValueService keyValueService, IContentService contentService)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
        }

        public bool SyncOne(string Shortcode)
        {
            var postsFromApi = ApiHelper.GetPosts();

            // TODO: Move this into a config service?
            var postsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.postsParentNodeId"));
            var postsParentNode = _contentService.GetById(postsParentNodeId);

            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(postsParentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(postsParentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("postShortcode") == Shortcode)
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
