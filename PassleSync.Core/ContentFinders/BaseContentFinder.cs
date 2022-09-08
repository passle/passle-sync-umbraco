using PassleSync.Core.Extensions;
using PassleSync.Core.Services;
using System;
using System.Linq;
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

        protected abstract string UrlPrefix { get; }
        protected abstract string ShortcodeName { get; }
        protected abstract string ContentType { get; }

        public bool TryFindContent(PublishedRequest request)
        {
            if (request.Uri.Segments.Length <= 1)
            {
                return false;
            }

            if (request.Uri.Segments.Skip(1).Take(1).SingleOrDefault() != $"{UrlPrefix}/")
            {
                return false;
            }

            var shortcode = request.Uri.Segments.Reverse().Skip(1).Take(1).SingleOrDefault().Trim('/');

            var virtualContentType = request.UmbracoContext.Content.GetContentType(ContentType);
            var virtualContent = request.UmbracoContext.Content.GetByContentType(virtualContentType);

            if (virtualContent == null)
            {
                return false;
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
