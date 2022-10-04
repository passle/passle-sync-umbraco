using PassleSync.Core.Attributes;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace PassleSync.Core.Controllers.Webhooks
{
    [PluginController("Passle")]
    public class HealthCheckController : UmbracoApiController
    {
        [HttpGet]
        [ValidateAPIKey]
        public IHttpActionResult Check()
        {
            return Ok();
        }
    }
}
