using System;
using System.Linq;
using Umbraco.Core.Services;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;

namespace PassleSync.Core.SyncHandlers
{
    public class AuthorHandler : SyncHandlerBase<PassleAuthors, PassleAuthor>
    {
        public AuthorHandler(
            IContentService contentService,
            ConfigService configService,
            PassleContentService<PassleAuthors, PassleAuthor> passleContentService,
            UmbracoContentService<PassleAuthor> umbracoContentService,
            ILogger logger
        ) : base(
            contentService,
            configService,
            passleContentService,
            umbracoContentService,
            logger
        )
        {
        }

        public override IPassleDashboardViewModel GetAll()
        {
            var peopleFromApi = _passleContentService.GetAll();
            if (peopleFromApi == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }

            var umbracoAuthors = _umbracoContentService.GetPublishedContent();

            // Create viewmodels
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            var apiAuthorModels = peopleFromApi.Select(author => new PassleDashboardAuthorViewModel(author));

            var umbracoShortcodes = umbracoAuthorModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoAuthorModels.Concat(apiAuthorModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardAuthorsViewModel(allModels);
        }

        public override string Shortcode(PassleAuthor item)
        {
            return item.Shortcode;
        }
    }
}
