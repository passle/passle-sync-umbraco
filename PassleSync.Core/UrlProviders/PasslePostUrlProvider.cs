﻿using PassleSync.Core.Constants;
using PassleSync.Core.Services;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Web.Routing;

namespace PassleSync.Core.UrlProviders
{
    public class PasslePostUrlProvider : BaseUrlProvider
    {
        public PasslePostUrlProvider(IRequestHandlerSection requestSettings, ILogger logger, IGlobalSettings globalSettings, ISiteDomainHelper siteDomainHelper, ConfigService configService) : base(requestSettings, logger, globalSettings, siteDomainHelper, configService)
        {
        }

        protected override string UrlPrefix => _configService.PostPermalinkPrefix;
        protected override string ShortcodeName => "PostShortcode";
        protected override string UrlName => "PostUrl";
        protected override string ContentType => PassleContentType.PASSLE_POST;
    }
}
