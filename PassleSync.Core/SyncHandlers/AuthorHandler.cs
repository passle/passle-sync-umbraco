using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.ViewModels.PassleDashboard;
using Umbraco.Core.Logging;
using PassleSync.Core.API.ViewModels;
using PassleSync.Core.Models;

namespace PassleSync.Core.SyncHandlers
{
    public class AuthorHandler : ISyncHandler<PassleAuthor>
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService;
        protected readonly ILogger _logger;


        public AuthorHandler(
            IKeyValueService keyValueService,
            IContentService contentService,
            ILogger logger)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
            _logger = logger;
        }

        public IPassleDashboardViewModel GetAll()
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }

            // TODO: Move this into a config service?
            int postsParentNodeId;
            try
            {
                postsParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(postsParentNodeId) == null)
                {
                    return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return new PassleDashboardAuthorsViewModel(Enumerable.Empty<PassleDashboardAuthorViewModel>());
            }

            var umbracoAuthors = GetAllUmbraco(postsParentNodeId);

            // Create viewmodels
            var umbracoAuthorModels = umbracoAuthors.Select(author => new PassleDashboardAuthorViewModel(author));
            var apiAuthorModels = peopleFromApi.People.Select(author => new PassleDashboardAuthorViewModel(author));

            var umbracoShortcodes = umbracoAuthorModels.Select(x => x.Shortcode);
            // Merge Enumerables
            var allModels = umbracoAuthorModels.Concat(apiAuthorModels.Where(x => !umbracoShortcodes.Contains(x.Shortcode)));

            return new PassleDashboardAuthorsViewModel(allModels);
        }

        public IEnumerable<IContent> GetAllUmbraco(int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                return _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();
            }
            return Enumerable.Empty<IContent>();
        }

        public bool SyncAll()
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get people from the API
                return false;
            }

            // TODO: Move this into a config service?
            int peopleParentNodeId;
            try
            {
                peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(peopleParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteAll(peopleParentNodeId);
            CreateAll(peopleFromApi.People, peopleParentNodeId);

            return true;
        }

        public bool SyncMany(string[] shortcodes)
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return false;
            }

            // TODO: Move this into a config service?
            int peopleParentNodeId;
            try
            {
                peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(peopleParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteMany(shortcodes, peopleParentNodeId);
            CreateMany(peopleFromApi.People, peopleParentNodeId, shortcodes);

            return true;
        }

        public bool DeleteAll()
        {
            int peopleParentNodeId;
            try
            {
                peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(peopleParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteAll(peopleParentNodeId);
            return true;
        }

        public void DeleteAll(int parentNodeId)
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

        public bool DeleteMany(string[] Shortcodes)
        {
            int peopleParentNodeId;
            try
            {
                peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(peopleParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            DeleteMany(Shortcodes, peopleParentNodeId);
            return true;
        }

        public void DeleteMany(string[] Shortcodes, int parentNodeId)
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

        public void DeleteOne(string Shortcode, int parentNodeId)
        {
            DeleteMany(new string[] { Shortcode }, parentNodeId);
        }

        public void CreateAll(IEnumerable<PassleAuthor> people, int parentNodeId)
        {
            foreach (PassleAuthor person in people)
            {
                CreateOne(person, parentNodeId);
            }
        }

        public void CreateMany(IEnumerable<PassleAuthor> people, int parentNodeId, string[] Shortcodes)
        {
            foreach (PassleAuthor person in people)
            {
                if (Shortcodes.Contains(person.Shortcode))
                {
                    CreateOne(person, parentNodeId);
                }
            }
        }

        public void CreateOne(PassleAuthor person, int parentNodeId)
        {
            // TODO: Const for "person"
            var node = _contentService.Create(person.Name, parentNodeId, "passleAuthor");

            // TODO: Should these strings be consts?
            // TODO: Capitalisation?
            node.SetValue("Shortcode", person.Shortcode);
            node.SetValue("Name", person.Name);
            node.SetValue("ProfileUrl", person.ProfileUrl);
            node.SetValue("Description", person.Description);
            node.SetValue("EmailAddress", person.EmailAddress);
            node.SetValue("PhoneNumber", person.PhoneNumber);
            node.SetValue("LinkedInProfileLink", person.LinkedInProfileLink);
            node.SetValue("FacebookProfileLink", person.FacebookProfileLink);
            node.SetValue("TwitterScreenName", person.TwitterScreenName);
            node.SetValue("XingProfileLink", person.XingProfileLink);
            node.SetValue("SkypeProfileLink", person.SkypeProfileLink);
            node.SetValue("VimeoProfileLink", person.VimeoProfileLink);
            node.SetValue("YouTubeProfileLink", person.YouTubeProfileLink);
            node.SetValue("StumbleUponProfileLink", person.StumbleUponProfileLink);
            node.SetValue("PinterestProfileLink", person.PinterestProfileLink);
            node.SetValue("InstagramProfileLink", person.InstagramProfileLink);
            node.SetValue("PersonalLinks", person.PersonalLinks);
            node.SetValue("LocationDetail", person.LocationDetail);
            node.SetValue("LocationCountry", person.LocationCountry);
            node.SetValue("TagLineCompany", person.TagLineCompany);
            node.SetValue("SubscribeLink", person.SubscribeLink);
            node.SetValue("AvatarUrl", person.AvatarUrl);
            node.SetValue("RoleInfo", person.RoleInfo);

            _contentService.SaveAndPublish(node);
        }

        public bool SyncOne(string Shortcode)
        {
            var peopleFromApi = ApiHelper.GetAuthors();
            if (peopleFromApi == null || peopleFromApi.People == null)
            {
                // Failed to get posts from the API
                return false;
            }

            // TODO: Move this into a config service?
            int peopleParentNodeId;
            try
            {
                peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PeopleParentNodeId"));

                if (_contentService.GetById(peopleParentNodeId) == null)
                {
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(_contentService.GetType(), ex, $"Failed to find umbraco content: {ex.Message}");
                return false;
            }

            var personFromApi = peopleFromApi.People.FirstOrDefault(x => x.Shortcode == Shortcode);
            if (personFromApi == null)
            {
                return false;
            }

            DeleteOne(Shortcode, peopleParentNodeId);
            CreateOne(personFromApi, peopleParentNodeId);

            return true;
        }
    }
}
