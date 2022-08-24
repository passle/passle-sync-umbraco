using PassleSync.Core.Extensions;
using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Models;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

namespace PassleSync.Core.Components
{
    public class RegisterDocumentTypesComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public RegisterDocumentTypesComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            // Create a migration plan for a specific project/feature
            var migrationPlan = new MigrationPlan("RegisterDocumentTypes");

            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty).To<RegisterDocumentTypes>("RegisterDocumentTypes");

            // Upgrade the site based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }

    public class RegisterDocumentTypes : MigrationBase
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        public RegisterDocumentTypes(IMigrationContext context, IContentTypeService contentTypeService, IDataTypeService dataTypeService) : base(context)
        {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
        }

        public override void Migrate()
        {
            Logger.Debug<RegisterDocumentTypes>("Running migration {MigrationStep}", "RegisterDocumentTypes");

            CreateDataTypes();
            CreatePasslePostContentType();
            CreatePassleAuthorContentType();
        }

        private void CreateDataTypes()
        {
            // TODO: Use constants for data type aliases
            if (_dataTypeService.GetDataType("Passle Repeatable Textstrings") == null)
            {
                var editor = Current.Factory.GetInstance<MultipleTextStringPropertyEditor>();
                var dataType = new DataType(editor)
                {
                    Name = "Passle Repeatable Textstrings",
                };

                _dataTypeService.Save(dataType);
            }
        }

        private void CreatePasslePostContentType()
        {
            if (_contentTypeService.Get("passlePost") != null)
            {
                return;
            }

            var passlePostContentType = new ContentType(-1)
            {
                Name = "Passle Post",
                Alias = "passlePost",
                Description = "A Passle post synced to your Umbraco instance.",
                Icon = "icon-newspaper color-deep-orange",
            };

            passlePostContentType.AddPropertyGroup("Content");

            // TODO: Use constants for data type aliases
            AddPropertyToContentType(passlePostContentType, "Textarea", "PostContentHtml");
            AddPropertyToContentType(passlePostContentType, "Textarea", "FeaturedItemHtml");
            AddPropertyToContentType(passlePostContentType, "Textstring", "FeaturedItemPosition");
            AddPropertyToContentType(passlePostContentType, "Textarea", "QuoteText");
            AddPropertyToContentType(passlePostContentType, "Textstring", "QuoteUrl");
            AddPropertyToContentType(passlePostContentType, "True/false", "IsFeaturedOnPasslePage");
            AddPropertyToContentType(passlePostContentType, "True/false", "IsFeaturedOnPostPage");
            AddPropertyToContentType(passlePostContentType, "Textarea", "PostShortcode");
            AddPropertyToContentType(passlePostContentType, "Textarea", "PassleShortcode");
            AddPropertyToContentType(passlePostContentType, "Textstring", "PostUrl");
            AddPropertyToContentType(passlePostContentType, "Textstring", "PostTitle");
            AddPropertyToContentType(passlePostContentType, "Textstring", "Authors"); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", "CoAuthors"); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", "ShareViews"); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textarea", "ContentTextSnippet");
            AddPropertyToContentType(passlePostContentType, "Date Picker", "PublishedDate");
            AddPropertyToContentType(passlePostContentType, "Textstring", "Tags"); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", "FeaturedItemMediaType");
            AddPropertyToContentType(passlePostContentType, "Textstring", "FeaturedItemEmbedType");
            AddPropertyToContentType(passlePostContentType, "Textarea", "FeaturedItemEmbedProvider");
            AddPropertyToContentType(passlePostContentType, "Textarea", "ImageUrl");
            AddPropertyToContentType(passlePostContentType, "Textstring", "TotalShares");
            AddPropertyToContentType(passlePostContentType, "True/false", "IsRepost");
            AddPropertyToContentType(passlePostContentType, "Textstring", "EstimatedReadTimeInSeconds");
            AddPropertyToContentType(passlePostContentType, "Textstring", "TotalLikes");
            AddPropertyToContentType(passlePostContentType, "True/false", "OpensInNewTab");
            AddPropertyToContentType(passlePostContentType, "Textstring", "Tweets"); // TODO: Is there a better data type for this?

            _contentTypeService.Save(passlePostContentType);
        }

        private void CreatePassleAuthorContentType()
        {
            if (_contentTypeService.Get("passleAuthor") != null)
            {
                return;
            }

            var passleAuthorContentType = new ContentType(-1)
            {
                Name = "Passle Author",
                Alias = "passleAuthor",
                Description = "A Passle author synced to your Umbraco instance.",
                Icon = "icon-user color-deep-orange",
            };

            passleAuthorContentType.AddPropertyGroup("Content");

            // TODO: Use constants for data type aliases
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "Shortcode");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "Name");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "ImageUrl");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "ProfileUrl");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "Role");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "Synced");
            AddPropertyToContentType(passleAuthorContentType, "Textarea", "Description");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "EmailAddress");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "PhoneNumber");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "LinkedInProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "FacebookProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "TwitterScreenName");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "XingProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "SkypeProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "VimeoProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "YouTubeProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "StumbleUponProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "PinterestProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "InstagramProfileLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "PersonalLinks");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "LocationDetail");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "LocationCountry");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "TagLineCompany");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "SubscribeLink");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "AvatarUrl");
            AddPropertyToContentType(passleAuthorContentType, "Textstring", "RoleInfo");

            _contentTypeService.Save(passleAuthorContentType);
        }

        private void AddPropertyToContentType(ContentType contentType, string dataTypeName, string propertyName)
        {
            var dataType = _dataTypeService.GetDataType(dataTypeName);
            var propertyType = new PropertyType(dataType, propertyName.FirstCharToLower())
            {
                Name = propertyName,
            };

            contentType.AddPropertyType(propertyType, "Content");
        }
    }
}
