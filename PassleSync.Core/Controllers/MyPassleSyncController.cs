using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using System.Web.Http;
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
        public ISyncHandler<Person> _peopleHandler;

        public MyPassleSyncController(
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
        public SettingsData Get()
        {
            var data = new SettingsData
            {
                Shortcode = _keyValueService.GetValue("PassleSync.Shortcode"),
                ApiKey = _keyValueService.GetValue("PassleSync.ApiKey"),
                ApiUrl = _keyValueService.GetValue("PassleSync.ApiUrl"),
                PluginApiKey = _keyValueService.GetValue("PassleSync.PluginApiKey"),
                PostPermalinkPrefix = _keyValueService.GetValue("PassleSync.PostPermalinkPrefix"),
                PersonPermalinkPrefix = _keyValueService.GetValue("PassleSync.PersonPermalinkPrefix")
            };

            if (!string.IsNullOrWhiteSpace(_keyValueService.GetValue("PassleSync.PeopleParentNodeId")))
            {
                data.PeopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));
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
        public IHttpActionResult Save(SettingsData timetable)
        {
            _keyValueService.SetValue("PassleSync.Shortcode", timetable.Shortcode);
            _keyValueService.SetValue("PassleSync.ApiKey", timetable.ApiKey);
            _keyValueService.SetValue("PassleSync.ApiUrl", timetable.ApiUrl);

            _keyValueService.SetValue("PassleSync.PluginApiKey", timetable.PluginApiKey);
            _keyValueService.SetValue("PassleSync.PostPermalinkPrefix", timetable.PostPermalinkPrefix);
            _keyValueService.SetValue("PassleSync.PersonPermalinkPrefix", timetable.PersonPermalinkPrefix);

            if (timetable.PeopleParentNodeId > 0)
            {
                _keyValueService.SetValue("PassleSync.PeopleParentNodeId", timetable.PeopleParentNodeId.ToString());
            }
            if (timetable.PostsParentNodeId > 0)
            {
                _keyValueService.SetValue("PassleSync.PostsParentNodeId", timetable.PostsParentNodeId.ToString());
            }

            // do something to persist timetable
            return Ok();
        }

    }
    public class SettingsData
    {
        public string Shortcode { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public string PluginApiKey { get; set; }
        public string PostPermalinkPrefix { get; set; }
        public string PersonPermalinkPrefix { get; set; }
        public int PeopleParentNodeId { get; set; }
        public int PostsParentNodeId { get; set; }
    }
}
