using PassleSync.Core.Constants;
using PassleSync.Core.Services;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Web.Routing;

namespace PassleSync.Core.UrlProviders
{
    public class PassleAuthorUrlProvider : BaseUrlProvider
    {
        public PassleAuthorUrlProvider(IRequestHandlerSection requestSettings, ILogger logger, IGlobalSettings globalSettings, ISiteDomainHelper siteDomainHelper, ConfigService configService) : base(requestSettings, logger, globalSettings, siteDomainHelper, configService)
        {
        }

        protected override string UrlPrefix => _configService.PersonPermalinkTemplate;
        protected override string ShortcodeName => "Shortcode";
        protected override string UrlName => "ProfileUrl";
        protected override string ContentType => PassleContentType.PASSLE_AUTHOR;
    }
}
