using PassleSync.Core.Constants;
using PassleSync.Core.Extensions;
using PassleSync.Core.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace PassleSync.Core.ContentFinders
{
    public abstract class BaseContentFinder : IContentFinder
    {
        protected readonly ConfigService _configService;

        public BaseContentFinder(ConfigService configService) : base()
        {
            _configService = configService;
        }

        protected abstract string UrlTemplate { get; }
        protected abstract string ShortcodeName { get; }
        protected abstract string ShortcodePlaceHolder { get; }
        protected abstract string ContentType { get; }

        public bool TryFindContent(PublishedRequest request)
        {
            if (request.Uri.Segments.Length <= 1)
            {
                return false;
            }

            var virtualContentType = request.UmbracoContext.Content.GetContentType(ContentType);
            var virtualContent = request.UmbracoContext.Content.GetByContentType(virtualContentType);
            
            if (virtualContent == null)
            {
                return false;
            }

            string shortcode = string.Empty;
            if (ContentType == PassleContentType.PASSLE_AUTHOR || ContentType == PassleContentType.PASSLE_POST) 
            {
                // Replace the template variable with a capture group to extract the shortcode
                var pattern = Regex.Replace(UrlTemplate, $"{{{{{ShortcodePlaceHolder}}}}}", $"(?<{ShortcodePlaceHolder}>[a-z0-9]+)");

                // Replace the remaining template variables with wildcards
                pattern = Regex.Replace(pattern, @"\{\{([a-zA-Z0-9]+)\}\}", "[a-z0-9\\-]+");

                // Create a regular expression object and match it against the request URI
                Regex regex = new Regex(pattern);
                Match match = regex.Match(request.Uri.ToString());

                if (match.Success)
                {
                    // Extract the shortcode value from the named group
                    shortcode = match.Groups[ShortcodePlaceHolder].Value;
                }
            }

            var content = virtualContent.FirstOrDefault(x => x.IsPublished() && x.GetValueOrDefault<string>(ShortcodeName) == shortcode);
            if (content == null)
            {
                return false;
            }

            request.PublishedContent = content;
            request.TrySetTemplate(content.GetTemplateAlias());
            return true;
        }
    }
}
