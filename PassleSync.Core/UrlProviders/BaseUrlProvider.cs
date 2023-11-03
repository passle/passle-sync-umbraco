using Microsoft.CodeAnalysis.CSharp.Syntax;
using PassleSync.Core.Constants;
using PassleSync.Core.Extensions;
using PassleSync.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Entities;
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

        protected abstract string UrlTemplate { get; }
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
                Dictionary<string, string> templateVariables = new Dictionary<string, string>();

                string path;
                if (content.ContentType.Alias == PassleContentType.PASSLE_POST)
                {
                    templateVariables.Add("{{PostShortcode}}", shortcode);
                    templateVariables.Add("{{PostSlug}}", slug);
                    path = Interpolate(url, templateVariables);

                }
                else if (content.ContentType.Alias == PassleContentType.PASSLE_AUTHOR)
                {
                    templateVariables.Add("{{PersonShortcode}}", shortcode);
                    templateVariables.Add("{{PersonSlug}}", slug);
                    path = Interpolate(url, templateVariables);

                }
                else
                {
                    var slug = url.Split('/').Last();
                    path = string.Join("/", shortcode, slug);
                }

                return new UrlInfo(path, true, defaultUrlInfo.Culture);
            }
            else
            {
                return defaultUrlInfo;
            }
        }

        private string Interpolate(string input, Dictionary<string, string> variables)
        {
            return variables.Aggregate(input, (current, value) =>
                current.Replace(value.Key, value.Value));
        }
    }
}
