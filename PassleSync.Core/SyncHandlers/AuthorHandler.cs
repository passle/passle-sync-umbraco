using System;
using System.Linq;
using Umbraco.Core.Services;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;
using System.Collections.Generic;
using System.Net.Http;
using PassleSync.Core.Exceptions;

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
            IEnumerable<PassleAuthor> peopleFromApi;
            try
            {
                peopleFromApi = _passleContentService.GetAll();
            }
            catch (PassleException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new PassleException(typeof(PassleAuthor), PassleExceptionEnum.UNKNOWN);
            }

            if (peopleFromApi == null)
            {
                // Failed to get people from the API
                throw new PassleException(typeof(PassleAuthor), PassleExceptionEnum.NULL_FROM_API);
            }

            var umbracoAuthors = _umbracoContentService.GetAllContent();

            // Create viewmodels
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            var apiAuthorModels = peopleFromApi.Select(author => new PassleDashboardAuthorViewModel(author));

            var umbracoShortcodes = umbracoAuthorModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoAuthorModels.Concat(apiAuthorModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardAuthorsViewModel(allModels);
        }

        public override IPassleDashboardViewModel GetExisting()
        {
            var umbracoAuthors = _umbracoContentService.GetAllContent();
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            return new PassleDashboardAuthorsViewModel(umbracoAuthorModels);
        }

        public override string Shortcode(PassleAuthor item)
        {
            return item.Shortcode;
        }
    }
}
