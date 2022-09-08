//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
using PassleSync.Core.Services;
using System;
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
        private readonly string _apiKey;
        private const string _apiKeyParamName = "APIKey";

        public ValidateAPIKeyAttribute()
        {
            var configService = Current.Factory.GetInstance<ConfigService>();
            _apiKey = configService.PluginApiKey;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains(_apiKeyParamName))
            {
                var apiKey = (string)actionContext.Request.Headers.GetValues(_apiKeyParamName).FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey) || apiKey != _apiKey)
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
