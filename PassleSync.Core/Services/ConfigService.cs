using PassleSync.Core.Models.Content.Umbraco;
using System;
using System.Collections.Generic;
using Umbraco.Core.Services;

namespace PassleSync.Core.Services
{
    public class ConfigService
    {
        private IKeyValueService _keyValueService;

        public ConfigService(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public string PasslePostContentTypeAlias
        {
            get => "passlePost";
        }
        public string PassleAuthorContentTypeAlias
        {
            get => "passleAuthor";
        }

        public string PassleDomain
        {
            get => "localhost";
        }

        public string ApiUrl
        {
            get => string.Format(
                "clientwebapi.passle.{0}",
                PassleDomain
            );
        }

        public string PluginApiKey
        { 
            get => _keyValueService.GetValue("PassleSync.PluginApiKey");
            set => _keyValueService.SetValue("PassleSync.PluginApiKey", value);
        }
        public string ClientApiKey
        {
            get => _keyValueService.GetValue("PassleSync.ApiKey");
            set => _keyValueService.SetValue("PassleSync.ApiKey", value);
        }
        public string PassleShortcodesString
        {
            get => _keyValueService.GetValue("PassleSync.Shortcode");
            set => _keyValueService.SetValue("PassleSync.Shortcode", value);
        }
        public IEnumerable<string> PassleShortcodes
        {
            get => PassleShortcodesString.Split(','); 
        }
        public string PostPermalinkPrefix
        {
            get => _keyValueService.GetValue("PassleSync.PostPermalinkPrefix");
            set => _keyValueService.SetValue("PassleSync.PostPermalinkPrefix", value);
        }
        public string AuthorPermalinkPrefix
        {
            get => _keyValueService.GetValue("PassleSync.PersonPermalinkPrefix");
            set => _keyValueService.SetValue("PassleSync.PersonPermalinkPrefix", value);
        }
        public string PostsParentNode
        {
            get => _keyValueService.GetValue("PassleSync.PostsParentNodeId");
            set => _keyValueService.SetValue("PassleSync.PostsParentNodeId", value);
        }
        public string AuthorsParentNode
        {
            get => _keyValueService.GetValue("PassleSync.PeopleParentNodeId");
            set => _keyValueService.SetValue("PassleSync.PeopleParentNodeId", value);
        }
        public int PostsParentNodeId
        {
            get
            {
                try
                {
                    return int.Parse(PostsParentNode);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public int AuthorsParentNodeId
        {
            get
            {
                try
                {
                    return int.Parse(AuthorsParentNode);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public void Update(SettingsData settings)
        {
            PassleShortcodesString = string.Join(",", settings.PassleShortcodes);
            ClientApiKey = settings.ClientApiKey;
            PluginApiKey = settings.PluginApiKey;
            PostPermalinkPrefix = settings.PostPermalinkPrefix;
            AuthorPermalinkPrefix = settings.AuthorPermalinkPrefix;
            PostsParentNode = settings.PostsParentNodeId.ToString();
            AuthorsParentNode = settings.AuthorsParentNodeId.ToString();
        }
    }
}
