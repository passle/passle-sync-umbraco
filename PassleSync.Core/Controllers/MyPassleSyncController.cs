using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using System.Web.Http;
using PassleDotCom.PasslePlugin.Core.Helpers;
using System.Collections.Generic;
using Umbraco.Core.Models;
using System.Linq;
using Umbraco.Core.Services.Implement;
using SubscribeToPublishEventComposer;

namespace PassleDotCom.PasslePlugin.Core.Controllers
{
    ///umbraco/backoffice/myPassleSync/myPassleSync/Save
    [PluginController("myPassleSync")]
    public class MyPassleSyncController : UmbracoAuthorizedApiController
    {
        private IScopeProvider _scopeProvider;
        private IMigrationBuilder _migrationBuilder;
        private IKeyValueService _keyValueService;
        private ILogger _logger;
        public IContentService _contentService { get; set; }
        private INotificationService _notificationService;


        public MyPassleSyncController(INotificationService notificationService, IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger, IContentService contentService)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
            _contentService = contentService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public SettingsData Get()
        {
            var data = new SettingsData();
            data.shortcode = _keyValueService.GetValue("Passle.shortcode");
            data.apiKey = _keyValueService.GetValue("Passle.apiKey");
            data.apiUrl = _keyValueService.GetValue("Passle.apiUrl");

            data.pluginApiKey = _keyValueService.GetValue("Passle.pluginApiKey");
            data.postPermalinkPrefix = _keyValueService.GetValue("Passle.postPermalinkPrefix");
            data.personPermalinkPrefix = _keyValueService.GetValue("Passle.personPermalinkPrefix");

            if (!string.IsNullOrWhiteSpace(_keyValueService.GetValue("Passle.peopleParentNodeId")))
            {
                data.peopleParentNodeId = int.Parse(_keyValueService.GetValue("Passle.peopleParentNodeId"));
            }
            if (!string.IsNullOrWhiteSpace(_keyValueService.GetValue("Passle.postsParentNodeId")))
            {
                data.postsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.postsParentNodeId"));
            }

            return data;
        }

        [HttpPost]
        public IHttpActionResult Sync()
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
                    _contentService.Delete(child);
                }
            }

            if (authorsParentNode != null && authors != null && authors.People != null)
            {
                foreach (var author in authors.People)
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
                    //node.SetValue("personalLinks", author.PersonalLinks);
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
            }

            var posts = ApiHelper.GetPosts();
            var postsParentNodeId = int.Parse(_keyValueService.GetValue("Passle.postsParentNodeId"));
            var postsParentNode = _contentService.GetById(postsParentNodeId);

            if (_contentService.HasChildren(postsParentNodeId))
            {
                long totalChildren;
                IEnumerable<IContent> children = _contentService.GetPagedChildren(postsParentNodeId, 0, 100, out totalChildren).ToList();

                foreach (var child in children)
                {
                    _contentService.Delete(child);
                }
            }


            if (postsParentNode != null && posts != null && posts.Posts != null)
            {
                foreach (var post in posts.Posts)
                {
                    var node = _contentService.Create(post.PostTitle, postsParentNode.Id, "post");

                    node.SetValue("PostContentHtml", post.PostContentHtml);
                    node.SetValue("FeaturedItemHtml", post.FeaturedItemHtml);
                    node.SetValue("FeaturedItemPosition", post.FeaturedItemPosition);
                    node.SetValue("QuoteText", post.QuoteText);


                    node.SetValue("QuoteUrl", post.QuoteUrl);
                    //node.SetValue("Tweets", post.Tweets);
                    node.SetValue("IsFeaturedOnPasslePage", post.IsFeaturedOnPasslePage);
                    node.SetValue("IsFeaturedOnPostPage", post.IsFeaturedOnPostPage);
                    node.SetValue("PostShortcode", post.PostShortcode);
                    node.SetValue("PassleShortcode", post.PassleShortcode);
                    node.SetValue("PostUrl", post.PostUrl);
                    node.SetValue("PostTitle", post.PostTitle);
                    //node.SetValue("Authors", post.Authors);
                    //node.SetValue("CoAuthors", post.CoAuthors);
                    //node.SetValue("ShareViews", post.ShareViews);
                    node.SetValue("ContentTextSnippet", post.ContentTextSnippet);
                    node.SetValue("PublishedDate", post.PublishedDate);
                    //node.SetValue("Tags", post.Tags);
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
            }


            // do something to persist timetable
            return Ok();
        }


        [HttpPost]
        public IHttpActionResult SyncPosts()
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
                    _contentService.Delete(child);
                }
            }


            if (postsParentNode != null && posts != null && posts.Posts != null)
            {
                foreach (var post in posts.Posts)
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
            }


            // do something to persist timetable
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SyncAuthors()
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
                    _contentService.Delete(child);
                }
            }

            if (authorsParentNode != null && authors != null && authors.People != null)
            {
                foreach (var author in authors.People)
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
                    //node.SetValue("personalLinks", author.PersonalLinks);
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
            }

            // do something to persist timetable
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Save(SettingsData timetable)
        {
            _keyValueService.SetValue("Passle.shortcode", timetable.shortcode);
            _keyValueService.SetValue("Passle.apiKey", timetable.apiKey);
            _keyValueService.SetValue("Passle.apiUrl", timetable.apiUrl);

            _keyValueService.SetValue("Passle.pluginApiKey", timetable.pluginApiKey);
            _keyValueService.SetValue("Passle.postPermalinkPrefix", timetable.postPermalinkPrefix);
            _keyValueService.SetValue("Passle.personPermalinkPrefix", timetable.personPermalinkPrefix);


            if (timetable.peopleParentNodeId > 0)
            {
                _keyValueService.SetValue("Passle.peopleParentNodeId", timetable.peopleParentNodeId.ToString());
            }
            if (timetable.postsParentNodeId > 0)
            {
                _keyValueService.SetValue("Passle.postsParentNodeId", timetable.postsParentNodeId.ToString());
            }

            // do something to persist timetable
            return Ok();
        }

    }
    public class SettingsData
    {
        public string shortcode { get; set; }
        public string apiKey { get; set; }
        public string apiUrl { get; set; }

        public string pluginApiKey { get; set; }
        public string postPermalinkPrefix { get; set; }
        public string personPermalinkPrefix { get; set; }
        public int peopleParentNodeId { get; set; }
        public int postsParentNodeId { get; set; }

    }
}
