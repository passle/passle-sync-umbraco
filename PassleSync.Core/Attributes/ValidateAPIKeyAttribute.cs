using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassleSync.Core.Services;
using System.Net;

namespace PassleSync.Core.Attributes
{
    public class ValidateAPIKeyAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly string _apiKey;

        public ValidateAPIKeyAttribute()
        {
            //_apiKey = ConfigService.ClientApiKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.HttpContext.Request.Headers.TryGetValue("APIKey", out var apiKey))
            //{
            //    if (string.IsNullOrEmpty(apiKey) || apiKey != _apiKey)
            //    {
            //        filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            //        return;
            //    }
            //}
            //else
            //{
            //    filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            //    return;
            //}
        }
    }
}
