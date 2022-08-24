using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using System.Web.Http;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;

namespace PassleSync.Core.Controllers
{
    // umbraco/backoffice/passleSync
    [PluginController("passleSync")]
    public class PassleSyncController : UmbracoAuthorizedApiController
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService;
        public ISyncHandler<Post> _postHandler;
        public ISyncHandler<Person> _peopleHandler;

        public PassleSyncController(
            IKeyValueService keyValueService,
            IContentService contentService,
            ISyncHandler<Post> postHandler,
            ISyncHandler<Person> peopleHandler)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
            _postHandler = postHandler;
            _peopleHandler = peopleHandler;
        }

        [HttpGet]
        public Settings Get()
        {
            var data = new Settings
            {
                PassleShortcodes = _keyValueService.GetValue("PassleSync.Shortcode"),
                ClientApiKey = _keyValueService.GetValue("PassleSync.ApiKey"),
                ApiUrl = _keyValueService.GetValue("PassleSync.ApiUrl"),
                PluginApiKey = _keyValueService.GetValue("PassleSync.PluginApiKey"),
                PostPermalinkPrefix = _keyValueService.GetValue("PassleSync.PostPermalinkPrefix"),
                AuthorPermalinkPrefix = _keyValueService.GetValue("PassleSync.PersonPermalinkPrefix")
            };

            if (!string.IsNullOrWhiteSpace(_keyValueService.GetValue("PassleSync.PeopleParentNodeId")))
            {
                data.AuthorsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));
            }

            if (!string.IsNullOrWhiteSpace(_keyValueService.GetValue("PassleSync.PostsParentNodeId")))
            {
                data.PostsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));
            }

            return data;
        }

        [HttpPost]
        public IHttpActionResult Sync()
        {
            var successful = false;

            successful |= _postHandler.SyncAll();
            successful |= _peopleHandler.SyncAll();

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
            if (_peopleHandler.SyncAll())
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody] Settings settings)
        {
            _keyValueService.SetValue("PassleSync.Shortcode", settings.PassleShortcodes);
            _keyValueService.SetValue("PassleSync.ApiKey", settings.ClientApiKey);
            _keyValueService.SetValue("PassleSync.ApiUrl", "http://clientwebapi.passle.localhost/");

            _keyValueService.SetValue("PassleSync.PluginApiKey", settings.PluginApiKey);
            _keyValueService.SetValue("PassleSync.PostPermalinkPrefix", settings.PostPermalinkPrefix);
            _keyValueService.SetValue("PassleSync.PersonPermalinkPrefix", settings.AuthorPermalinkPrefix);

            if (settings.AuthorsParentNodeId > 0)
            {
                _keyValueService.SetValue("PassleSync.PeopleParentNodeId", settings.AuthorsParentNodeId.ToString());
            }
            if (settings.PostsParentNodeId > 0)
            {
                _keyValueService.SetValue("PassleSync.PostsParentNodeId", settings.PostsParentNodeId.ToString());
            }

            return Ok();
        }
    }

    public class Settings
    {
        public string PassleShortcodes { get; set; }
        public string ClientApiKey { get; set; }
        public string ApiUrl { get; set; }
        public string PluginApiKey { get; set; }
        public string PostPermalinkPrefix { get; set; }
        public string AuthorPermalinkPrefix { get; set; }
        public int PostsParentNodeId { get; set; }
        public int AuthorsParentNodeId { get; set; }
    }
}
