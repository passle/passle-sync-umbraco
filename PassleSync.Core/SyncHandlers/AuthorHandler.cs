using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PassleSync.Core.Helpers;
using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Admin;

namespace PassleSync.Core.SyncHandlers
{
    public class AuthorHandler : ISyncHandler<Person>
    {
        private IKeyValueService _keyValueService;
        public IContentService _contentService { get; set; }


        public AuthorHandler(IKeyValueService keyValueService, IContentService contentService)
        {
            _keyValueService = keyValueService;
            _contentService = contentService;
        }

        public bool SyncAll()
        {
            var peopleFromApi = ApiHelper.GetPosts();
            if (peopleFromApi == null || peopleFromApi.Posts == null)
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
            catch (ArgumentNullException)
            {
                return false;
            }

            DeleteAll(peopleParentNodeId);
            CreateMany(peopleFromApi.People, peopleParentNodeId);

            return true;
        }

        public bool SyncMany(string[] shortcodes)
        {
            var peopleFromApi = ApiHelper.GetPosts();
            if (peopleFromApi == null || peopleFromApi.Posts == null)
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
            catch (ArgumentNullException)
            {
                return false;
            }

            DeleteMany(shortcodes, peopleParentNodeId);
            CreateMany(peopleFromApi.People, peopleParentNodeId);

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

        public void DeleteMany(string[] Shortcodes, int parentNodeId)
        {
            // Delete any existing posts with the same shortcode
            if (_contentService.HasChildren(parentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(parentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (Shortcodes.Contains(child.GetValue<string>("postShortcode")))
                    {
                        _contentService.Delete(child);
                    }
                }
            }
        }

        public void CreateMany(IEnumerable<Person> people, int parentNodeId)
        {
            foreach (Person person in people)
            {
                CreateOne(person, parentNodeId);
            }
        }

        public void CreateOne(Person person, int parentNodeId)
        {
            // TODO: Const for "person"
            var node = _contentService.Create(person.Name, parentNodeId, "person");

            // TODO: Should these strings be consts?
            // TODO: Capitalisation?
            node.SetValue("Shortcode", person.Shortcode);
            node.SetValue("Name", person.Name);
            node.SetValue("ImageUrl", person.ImageUrl);
            node.SetValue("ProfileUrl", person.ProfileUrl);
            node.SetValue("Role", person.Role);
            node.SetValue("Synced", person.Synced);
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

        public bool SyncOne(string shortcode)
        {
            var peopleFromApi = ApiHelper.GetAuthors();

            // TODO: Move this into a config service?
            var peopleParentNodeId = int.Parse(_keyValueService.GetValue("PassleSync.PostsParentNodeId"));
            var peopleParentNode = _contentService.GetById(peopleParentNodeId);

            // Delete any existing people with the same shortcode
            if (_contentService.HasChildren(peopleParentNodeId))
            {
                IEnumerable<IContent> children = _contentService.GetPagedChildren(peopleParentNodeId, 0, 100, out long totalChildren).ToList();

                foreach (var child in children)
                {
                    if (child.GetValue<string>("Shortcode") == shortcode)
                    {
                        _contentService.Delete(child);
                    }
                }
            }

            // Create a new person
            var person = peopleFromApi.People.FirstOrDefault(x => x.Shortcode == shortcode);
            if (person != null)
            {
                // TODO: Const for "person"
                var node = _contentService.Create(person.Name, peopleParentNode.Id, "person");

                // TODO: Should these strings be consts?
                // TODO: Capitalisation?
                node.SetValue("Shortcode", person.Shortcode);
                node.SetValue("Name", person.Name);
                node.SetValue("ImageUrl", person.ImageUrl);
                node.SetValue("ProfileUrl", person.ProfileUrl);
                node.SetValue("Role", person.Role);
                node.SetValue("Synced", person.Synced);
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

                return true;
            }
            else
            {
                // Clearly we've not managed to delete all the existing posts if one still exists
                return false;
            }
        }
    }
}
