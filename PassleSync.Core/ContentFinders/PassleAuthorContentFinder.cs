using PassleSync.Core.Constants;
using PassleSync.Core.Services;

namespace PassleSync.Core.ContentFinders
{
    public class PassleAuthorContentFinder : BaseContentFinder
    {
        public PassleAuthorContentFinder(ConfigService configService) : base(configService)
        {
        }

        protected override string UrlPrefix => _configService.PersonPermalinkTemplate;
        protected override string ShortcodeName => "Shortcode";
        protected override string ContentType => PassleContentType.PASSLE_AUTHOR;
    }
}
