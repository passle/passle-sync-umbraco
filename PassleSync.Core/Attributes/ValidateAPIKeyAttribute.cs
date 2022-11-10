using PassleSync.Core.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Umbraco.Core;
using Umbraco.Web.Composing;

namespace PassleSync.Core.Attributes
{
    public class ValidateAPIKeyAttribute : ActionFilterAttribute, IActionFilter
    {
        private const string _apiKeyParamName = "APIKey";
        private ConfigService _configService;

        public ValidateAPIKeyAttribute()
        {
            _configService = Current.Factory.GetInstance<ConfigService>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var apiKey = _configService.PluginApiKey;

            if (actionContext.Request.Headers.Contains(_apiKeyParamName))
            {
                var requestApiKey = (string)actionContext.Request.Headers.GetValues(_apiKeyParamName).FirstOrDefault();
                if (string.IsNullOrEmpty(requestApiKey) || requestApiKey != apiKey)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    return;
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
