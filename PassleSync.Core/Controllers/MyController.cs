using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Helpers;
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
        public IContentService _contentService;
        public ISyncHandler _postHandler;


        public MyController(IKeyValueService keyValueService, IContentService contentService, ISyncHandler postHandler)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
            _postHandler = postHandler;
        }


        [HttpPost]
        public IHttpActionResult SyncPost(PostShortcodeModel post)
        {
            if (_postHandler.SyncOne(post.Shortcode))
            {
                return Ok();
            }
            else
            {
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
