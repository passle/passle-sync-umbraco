using PassleSync.Core.Constants;
using PassleSync.Core.Services;

namespace PassleSync.Core.ContentFinders
{
    public class PasslePostContentFinder : BaseContentFinder
    {
        public PasslePostContentFinder(ConfigService configService) : base(configService)
        {
        }

        protected override string UrlPrefix => _configService.PostPermalinkPrefix;
        protected override string ShortcodeName => "PostShortcode";
        protected override string ContentType => PassleContentType.PASSLE_POST;
    }
}
