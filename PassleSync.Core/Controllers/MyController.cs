using PassleDotCom.PasslePlugin.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace PassleSync.Core.Controllers
{
    public class MyController : Umbraco.Web.WebApi.UmbracoApiController
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService { get; set; }


        public MyController(IKeyValueService keyValueService, IContentService contentService)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
        }


        [HttpPost]
        public IHttpActionResult SyncPost(PostShortcodeModel post)
        {
            var syncedPosts = ApiHelper.GetPosts();

            // TODO: Move this into a config service?
            var postsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.postsParentNodeId"));
            var postsParentNode = _contentService.GetById(postsParentNodeId);

            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(postsParentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(postsParentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("postShortcode") == post.Shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            // Create a new post
            var syncedPost = syncedPosts.Posts.FirstOrDefault(x => x.PostShortcode == post.Shortcode);
            if (syncedPost != null)
            {
                // TODO: Const for "post"
                var node = _contentService.Create(syncedPost.PostTitle, postsParentNode.Id, "post");

                // TODO: Should these strings be consts?
                // TODO: Capitalisation?
                node.SetValue("PostContentHtml", syncedPost.PostContentHtml);
                node.SetValue("FeaturedItemHtml", syncedPost.FeaturedItemHtml);
                node.SetValue("FeaturedItemPosition", syncedPost.FeaturedItemPosition);
                node.SetValue("QuoteText", syncedPost.QuoteText);
                node.SetValue("QuoteUrl", syncedPost.QuoteUrl);
                node.SetValue("Tweets", syncedPost.Tweets);
                node.SetValue("IsFeaturedOnPasslePage", syncedPost.IsFeaturedOnPasslePage);
                node.SetValue("IsFeaturedOnPostPage", syncedPost.IsFeaturedOnPostPage);
                node.SetValue("PostShortcode", syncedPost.PostShortcode);
                node.SetValue("PassleShortcode", syncedPost.PassleShortcode);
                node.SetValue("PostUrl", syncedPost.PostUrl);
                node.SetValue("PostTitle", syncedPost.PostTitle);
                node.SetValue("Authors", syncedPost.Authors);
                node.SetValue("CoAuthors", syncedPost.CoAuthors);
                node.SetValue("ShareViews", syncedPost.ShareViews);
                node.SetValue("ContentTextSnippet", syncedPost.ContentTextSnippet);
                node.SetValue("PublishedDate", syncedPost.PublishedDate);
                node.SetValue("Tags", syncedPost.Tags);
                node.SetValue("FeaturedItemMediaType", syncedPost.FeaturedItemMediaType);
                node.SetValue("FeaturedItemEmbedType", syncedPost.FeaturedItemEmbedType);
                node.SetValue("FeaturedItemEmbedProvider", syncedPost.FeaturedItemEmbedProvider);
                node.SetValue("ImageUrl", syncedPost.ImageUrl);
                node.SetValue("TotalShares", syncedPost.TotalShares);
                node.SetValue("IsRepost", syncedPost.IsRepost);
                node.SetValue("EstimatedReadTimeInSeconds", syncedPost.EstimatedReadTimeInSeconds);
                node.SetValue("TotalLikes", syncedPost.TotalLikes);
                node.SetValue("OpensInNewTab", syncedPost.OpensInNewTab);

                _contentService.SaveAndPublish(node);

                // do something to persist timetable
                // TODO: What does this mean?
                return Ok();
            }
            else
            {
                // Clearly we've not managed to delete all the existing posts if one still exists
                return BadRequest();
            }
        }


        [HttpPost]
        public IHttpActionResult SyncAuthor(AuthorShortcodeModel author)
        {
            var syncedAuthors = ApiHelper.GetAuthors();

            // TODO: Move this into a config service?
            var authorsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.peopleParentNodeId"));
            var authorsParentNode = _contentService.GetById(authorsParentNodeId);

            // Delete any existing authors with the same shortcode
            if (_contentService.HasChildren(authorsParentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(authorsParentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("shortcode") == author.Shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            // Create a new author
            var syncedAuthor = syncedAuthors.People.FirstOrDefault(x => x.Shortcode == author.Shortcode);
            if (syncedAuthor != null)
            {
                // TODO: Const for "person"
                var node = _contentService.Create(syncedAuthor.Name, authorsParentNode.Id, "person");

                // TODO: Should these strings be consts?
                // TODO: Capitalisation?
                node.SetValue("description", syncedAuthor.Description);
                node.SetValue("shortcode", syncedAuthor.Shortcode);
                node.SetValue("imageUrl", syncedAuthor.ImageUrl);
                node.SetValue("roleInfo", syncedAuthor.RoleInfo);
                node.SetValue("avatarUrl", syncedAuthor.AvatarUrl);
                node.SetValue("subscribeLink", syncedAuthor.SubscribeLink);
                node.SetValue("tagLineCompany", syncedAuthor.TagLineCompany);
                node.SetValue("locationCountry", syncedAuthor.LocationCountry);
                node.SetValue("locationDetail", syncedAuthor.LocationDetail);
                node.SetValue("personalLinks", syncedAuthor.PersonalLinks);
                node.SetValue("instagramProfileLink", syncedAuthor.InstagramProfileLink);
                node.SetValue("pinterestProfileLink", syncedAuthor.PinterestProfileLink);
                node.SetValue("stumbleUponProfileLink", syncedAuthor.StumbleUponProfileLink);
                node.SetValue("youTubeProfileLink", syncedAuthor.YouTubeProfileLink);
                node.SetValue("vimeoProfileLink", syncedAuthor.VimeoProfileLink);
                node.SetValue("skypeProfileLink", syncedAuthor.SkypeProfileLink);
                node.SetValue("xingProfileLink", syncedAuthor.XingProfileLink);
                node.SetValue("facebookProfileLink", syncedAuthor.FacebookProfileLink);
                node.SetValue("linkedInProfileLink", syncedAuthor.LinkedInProfileLink);
                node.SetValue("phoneNumber", syncedAuthor.PhoneNumber);
                node.SetValue("emailAddress", syncedAuthor.EmailAddress);
                node.SetValue("twitterScreenName", syncedAuthor.TwitterScreenName);
                node.SetValue("profileUrl", syncedAuthor.ProfileUrl);

                _contentService.SaveAndPublish(node);

                // do something to persist timetable
                // TODO: What does this mean?
                return Ok();
            }
            else
            {
                // Clearly we've not managed to delete all the existing authors if one still exists
                return BadRequest();
            }
        }
    }

    public class SyncableObjectModel
    {
        public string Shortcode { get; set; }
    }

    public class PostShortcodeModel : SyncableObjectModel
    { }

    public class AuthorShortcodeModel : SyncableObjectModel
    { }

}
