using PassleDotCom.PasslePlugin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core.Migrations;
using Umbraco.Core.Models;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace PassleDotCom.PasslePlugin.Core.Controllers
{
    public class MyController : Umbraco.Web.WebApi.UmbracoApiController
    {
        private IScopeProvider _scopeProvider;
        private IMigrationBuilder _migrationBuilder;
        private IKeyValueService _keyValueService;
        public IContentService _contentService { get; set; }
        private INotificationService _notificationService;


        public MyController(INotificationService notificationService, IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, IContentService contentService)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _contentService = contentService;
            _notificationService = notificationService;
        }


        [HttpPost]
        public IHttpActionResult SyncPost(postingModel model)
        {
            var posts = ApiHelper.GetPosts();
            var postsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.postsParentNodeId"));
            var postsParentNode = _contentService.GetById(postsParentNodeId);


            if (_contentService.HasChildren(postsParentNodeId))
            {
                long totalChildren;
                IEnumerable<IContent> children = _contentService.GetPagedChildren(postsParentNodeId, 0, 100, out totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("postShortcode") == model.shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            var post = posts.Posts.FirstOrDefault(x => x.PostShortcode == model.shortcode);


            if (post != null)
            {

                var node = _contentService.Create(post.PostTitle, postsParentNode.Id, "post");

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


            // do something to persist timetable
            return Ok();
        }


        [HttpPost]
        public IHttpActionResult SyncAuthor(postingModel model)
        {
            var authors = ApiHelper.GetAuthors();
            var authorsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.peopleParentNodeId"));
            var authorsParentNode = _contentService.GetById(authorsParentNodeId);

            if (_contentService.HasChildren(authorsParentNodeId))
            {
                long totalChildren;
                IEnumerable<IContent> children = _contentService.GetPagedChildren(authorsParentNodeId, 0, 100, out totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("shortcode") == model.shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            var author = authors.People.FirstOrDefault(x => x.Shortcode == model.shortcode);

            if (author != null)
            {
                var node = _contentService.Create(author.Name, authorsParentNode.Id, "person");
                node.SetValue("description", author.Description);

                node.SetValue("shortcode", author.Shortcode);
                node.SetValue("imageUrl", author.ImageUrl);
                node.SetValue("roleInfo", author.RoleInfo);
                node.SetValue("avatarUrl", author.AvatarUrl);
                node.SetValue("subscribeLink", author.SubscribeLink);
                node.SetValue("tagLineCompany", author.TagLineCompany);
                node.SetValue("locationCountry", author.LocationCountry);
                node.SetValue("locationDetail", author.LocationDetail);
                node.SetValue("personalLinks", author.PersonalLinks);
                node.SetValue("instagramProfileLink", author.InstagramProfileLink);
                node.SetValue("pinterestProfileLink", author.PinterestProfileLink);
                node.SetValue("stumbleUponProfileLink", author.StumbleUponProfileLink);
                node.SetValue("youTubeProfileLink", author.YouTubeProfileLink);
                node.SetValue("vimeoProfileLink", author.VimeoProfileLink);
                node.SetValue("skypeProfileLink", author.SkypeProfileLink);
                node.SetValue("xingProfileLink", author.XingProfileLink);
                node.SetValue("facebookProfileLink", author.FacebookProfileLink);
                node.SetValue("linkedInProfileLink", author.LinkedInProfileLink);
                node.SetValue("phoneNumber", author.PhoneNumber);
                node.SetValue("emailAddress", author.EmailAddress);
                node.SetValue("twitterScreenName", author.TwitterScreenName);
                node.SetValue("profileUrl", author.ProfileUrl);

                _contentService.SaveAndPublish(node);

            }

            // do something to persist timetable
            return Ok();
        }



    }

    public class postingModel
    {
        public string shortcode { get; set; }
    }

}
