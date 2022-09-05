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

            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }

            var umbracoAuthors = GetAllUmbraco(peopleParentNodeId);

            // Create viewmodels
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            var apiAuthorModels = peopleFromApi.People.Select(author => new PassleDashboardAuthorViewModel(author));

            var umbracoShortcodes = umbracoAuthorModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoAuthorModels.Concat(apiAuthorModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardAuthorsViewModel(allModels);
        }

        private IEnumerable<IContent> GetAllUmbraco(int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                return _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
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
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    _contentService.Delete(child);
                }
            }
        }

        public override bool DeleteMany(string[] Shortcodes)
        {
            int peopleParentNodeId = _configService.AuthorsParentNodeId;
            if (_contentService.GetById(peopleParentNodeId) == null)
            {
                return false;
            }

            DeleteMany(Shortcodes, peopleParentNodeId);
            return true;
        }

        public override void DeleteMany(string[] Shortcodes, int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (Shortcodes.Contains(child.GetValue<string>("shortcode")))
                    {
                        _contentService.Delete(child);
                    }
                }
            }
        }

        private void DeleteOne(string Shortcode, int parentNodeId)
        {
            DeleteMany(new string[] { Shortcode }, parentNodeId);
        }

        public override void CreateAll(IEnumerable<PassleAuthor> people, int parentNodeId)
        {
            foreach (PassleAuthor person in people)
            {
                CreateOne(person, parentNodeId);
            }
        }

        public override void CreateMany(IEnumerable<PassleAuthor> people, int parentNodeId, string[] shortcodes)
        {
            foreach (PassleAuthor person in people)
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

            var publishedContent = _umbracoContentService.GetPublishedAuthorByShortcode(shortcode);
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
