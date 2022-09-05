using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using PassleSync.Core.Services.Content;
using PassleSync.Core.Extensions;

namespace PassleSync.Core.SyncHandlers
{
    public class AuthorHandler : SyncHandlerBase<PassleAuthor>
    {
        public AuthorHandler(IContentService contentService, UmbracoContentService umbracoContentService, ConfigService configService, ILogger logger) : base(contentService, umbracoContentService, configService, logger)
        {
        }

        public override IPassleDashboardViewModel GetAll()
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }

            var umbracoAuthors = _umbracoContentService.GetAuthors();

            // Create viewmodels
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            var apiAuthorModels = peopleFromApi.People.Select(author => new PassleDashboardAuthorViewModel(author));

            var umbracoShortcodes = umbracoAuthorModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoAuthorModels.Concat(apiAuthorModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardAuthorsViewModel(allModels);
        }

        public override bool SyncAll()
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get people from the API
                return false;
            }

            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            DeleteAll(peopleParentNodeId);
            CreateAll(peopleFromApi.People, peopleParentNodeId);

            return true;
        }

        public override bool SyncMany(string[] shortcodes)
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return false;
            }

            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(shortcodes, peopleParentNodeId);
            CreateMany(peopleFromApi.People, peopleParentNodeId, shortcodes);

            return true;
        }

        public override bool DeleteAll()
        {
            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            DeleteAll(peopleParentNodeId);
            return true;
        }

        public override void DeleteAll(int parentNodeId)
        {
            var authors = _umbracoContentService.GetAuthors();
            
            foreach (var author in authors)
            {
                _contentService.Delete(author);
            }
        }

        public override bool DeleteMany(string[] shortcodes)
        {
            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(shortcodes, peopleParentNodeId);
            return true;
        }

        public override void DeleteMany(string[] shortcodes, int parentNodeId)
        {
            var authors = _umbracoContentService.GetAuthors().Where(x => shortcodes.Contains(x.GetValueOrDefault<string>("Shortcode")));
                
            foreach (var author in authors)
            {
                _contentService.Delete(author);
            }
        }

        public override void CreateAll(IEnumerable<PassleAuthor> people, int parentNodeId)
        {
            foreach (var person in people)
            {
                CreateOne(person, parentNodeId);
            }
        }

        public override void CreateMany(IEnumerable<PassleAuthor> people, int parentNodeId, string[] shortcodes)
        {
            foreach (var person in people)
            {
                if (shortcodes.Contains(person.Shortcode))
                {
                    CreateOne(person, parentNodeId);
                }
            }
        }

        public override void CreateOne(PassleAuthor person, int parentNodeId)
        {
            var node = _contentService.Create(person.Name, parentNodeId, _configService.PassleAuthorContentTypeAlias);

            AddAllPropertiesToNode(node, person);

            _contentService.SaveAndPublish(node);
        }

        public override bool SyncOne(string shortcode)
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return false;
            }

            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            var personFromApi = peopleFromApi.People.FirstOrDefault(x => x.Shortcode == shortcode);
            if (personFromApi == null)
            {
                return false;
            }

            var publishedContent = _umbracoContentService.GetAuthorByShortcode(shortcode);
            if (publishedContent == null)
            {
                CreateOne(personFromApi, peopleParentNodeId);
            }
            else
            {
                var editableContent = _contentService.GetById(publishedContent.Id);

                editableContent.Name = personFromApi.Name;
                AddAllPropertiesToNode(editableContent, personFromApi);

                _contentService.SaveAndPublish(editableContent, raiseEvents: false);
            }

            return true;
        }
    }
}
