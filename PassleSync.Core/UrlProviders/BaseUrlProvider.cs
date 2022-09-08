using PassleSync.Core.Extensions;
using PassleSync.Core.Services;
using System;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace PassleSync.Core.UrlProviders
{
    public abstract class BaseUrlProvider : DefaultUrlProvider
    {
        protected readonly ConfigService _configService;

        protected BaseUrlProvider(IRequestHandlerSection requestSettings, ILogger logger, IGlobalSettings globalSettings, ISiteDomainHelper siteDomainHelper, ConfigService configService) : base(requestSettings, logger, globalSettings, siteDomainHelper)
        {
            _configService = configService;
        }

        protected abstract string UrlPrefix { get; }
        protected abstract string ShortcodeName { get; }
        protected abstract string UrlName { get; }
        protected abstract string ContentType { get; }

        public override UrlInfo GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            var defaultUrlInfo = base.GetUrl(umbracoContext, content, mode, culture, current);

            if (content == null || content.ContentType.Alias != ContentType)
            {
                return null;
            }

            if (defaultUrlInfo.IsUrl)
            {
                var shortcode = content.GetValueOrDefault<string>(ShortcodeName);
                var url = content.GetValueOrDefault<string>(UrlName);

                var slug = url.Split('/').Last();
                var path = $"/{string.Join("/", new string[] { UrlPrefix, shortcode, slug })}/";

                return new UrlInfo(path, true, defaultUrlInfo.Culture);
            }
            else
            {
                return defaultUrlInfo;
            }
        }
    }
}
