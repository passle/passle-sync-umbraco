using Examine;
using PassleSync.Core.API.Services;
using PassleSync.Core.Constants;
using PassleSync.Core.Models.Content.Umbraco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Examine;
using Umbraco.Web;
using UmbracoConstants = Umbraco.Core.Constants;

namespace PassleSync.Core.Services
{
    public class PassleHelperService : IPassleHelperService
    {
        private readonly IExamineManager _examineManager;
        private readonly UmbracoHelper _umbracoHelper;

        public PassleHelperService(IExamineManager examineManager, UmbracoHelper umbracoHelper)
        {
            _examineManager = examineManager;
            _umbracoHelper = umbracoHelper;

            // TODO: Extend models as per documenation instead of using viewmodel classes
            // https://our.umbraco.com/Documentation/Reference/Templating/Modelsbuilder/Understand-And-Extend-vpre8_5
        }

        public IEnumerable<PasslePost> GetPosts()
        {
            // TODO: Options for pagination, include/exclude featured post, search, tagging, etc.

            var posts = GetPublishedContent(PassleContentType.PASSLE_POST).Select(x => new PasslePost(x));
            return posts;
        }

        public IEnumerable<PassleAuthor> GetAuthors()
        {
            // TODO: Options for pagination, search, etc.

            var authors = GetPublishedContent(PassleContentType.PASSLE_AUTHOR).Select(x => new PassleAuthor(x));
            return authors;
        }

        private IEnumerable<IPublishedContent> GetPublishedContent(string contentType)
        {
            if (_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                var ids = index.GetSearcher().CreateQuery("content").NodeTypeAlias(contentType).Execute().Select(x => int.Parse(x.Id));

                foreach (var id in ids)
                {
                    yield return _umbracoHelper.Content(id);
                }
            }
        }
    }
}
