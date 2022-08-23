using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using System.Web.Http;
using PassleSync.Core.Helpers;
using System.Collections.Generic;
using Umbraco.Core.Models;
using System.Linq;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;

namespace PassleSync.Core.Controllers
{
    ///umbraco/backoffice/myPassleSync/myPassleSync/Save
    [PluginController("myPassleSync")]
    public class MyPassleSyncController : UmbracoAuthorizedApiController
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService;
        public ISyncHandler<Post> _postHandler;


        public MyPassleSyncController(
            IKeyValueService keyValueService,
            IContentService contentService,
            ISyncHandler<Post> postHandler)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
            _postHandler = postHandler;
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
            bool successful = false;

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


            successful |= _postHandler.SyncAll();

            if (successful)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            } 
                
        }


        [HttpPost]
        public IHttpActionResult SyncPosts()
        {
            if (_postHandler.SyncAll())
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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
