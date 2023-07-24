using PassleSync.Core.Models.Content.Umbraco;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Services;
using System.Configuration;

namespace PassleSync.Core.Services
{
    public class ConfigService
    {
        private IKeyValueService _keyValueService;
        private string _domain;

        public ConfigService(IKeyValueService keyValueService)
        {
            _domain = ConfigurationManager.AppSettings.Get("PASSLESYNC_DOMAIN") ?? "net";
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
            get => _domain;
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
            get => PassleShortcodesString?.Split(',') ?? Enumerable.Empty<string>(); 
        }
        public string PostPermalinkTemplate
        {
            get => _keyValueService.GetValue("PassleSync.PostPermalinkTemplate");
            set => _keyValueService.SetValue("PassleSync.PostPermalinkTemplate", value);
        }
        public string PersonPermalinkTemplate
        {
            get => _keyValueService.GetValue("PassleSync.PersonPermalinkTemplate");
            set => _keyValueService.SetValue("PassleSync.PersonPermalinkTemplate", value);
        }
        public string PreviewPermalinkTemplate
        {
            get => _keyValueService.GetValue("PassleSync.PreviewPermalinkTemplate");
            set => _keyValueService.SetValue("PassleSync.PreviewPermalinkTemplate", value);
        }
        public string SimulateRemoteHosting
        {
            get => _keyValueService.GetValue("PassleSync.SimulateRemoteHosting");
            set => _keyValueService.SetValue("PassleSync.SimulateRemoteHosting", value);
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
            PostPermalinkTemplate = settings.PostPermalinkTemplate;
            PersonPermalinkTemplate = settings.PersonPermalinkTemplate;
            PreviewPermalinkTemplate = settings.PreviewPermalinkTemplate;
            SimulateRemoteHosting = settings.SimulateRemoteHosting ? "True" : "False";
            PostsParentNode = settings.PostsParentNodeId.ToString();
            AuthorsParentNode = settings.AuthorsParentNodeId.ToString();
        }
    }
}
