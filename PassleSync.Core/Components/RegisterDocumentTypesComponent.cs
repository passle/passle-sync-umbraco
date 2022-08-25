using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
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
        private readonly ConfigService _configService;

        public RegisterDocumentTypes(
            IMigrationContext context,
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService,
            ConfigService configService) : base(context)
        {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _configService = configService;
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
            if (_contentTypeService.Get(_configService.PasslePostContentTypeAlias) != null)
            {
                return;
            }

            var passlePostContentType = new ContentType(-1)
            {
                Name = "Passle Post",
                Alias = _configService.PasslePostContentTypeAlias,
                Description = "A Passle post synced to your Umbraco instance.",
                Icon = "icon-newspaper color-deep-orange",
            };

            passlePostContentType.AddPropertyGroup("Content");

            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.PostTitleProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.PostShortcodeProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.PassleShortcodeProperty);
            AddPropertyToContentType(passlePostContentType, "Date Picker", PasslePost.PublishedDateProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.PostUrlProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.ImageUrlProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.AuthorsProperty); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.CoAuthorsProperty); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.PostContentHtmlProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.ContentTextSnippetProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.QuoteTextProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.QuoteUrlProperty);
            AddPropertyToContentType(passlePostContentType, "True/false", PasslePost.IsRepostProperty);
            AddPropertyToContentType(passlePostContentType, "True/false", PasslePost.IsFeaturedOnPasslePageProperty);
            AddPropertyToContentType(passlePostContentType, "True/false", PasslePost.IsFeaturedOnPostPageProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.EstimatedReadTimeInSecondsProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.FeaturedItemHtmlProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.FeaturedItemPositionProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.FeaturedItemMediaTypeProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.FeaturedItemEmbedTypeProperty);
            AddPropertyToContentType(passlePostContentType, "Textarea", PasslePost.FeaturedItemEmbedProviderProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.TagsProperty); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.TweetsProperty); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.ShareViewsProperty); // TODO: Is there a better data type for this?
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.TotalSharesProperty);
            AddPropertyToContentType(passlePostContentType, "Textstring", PasslePost.TotalLikesProperty);
            AddPropertyToContentType(passlePostContentType, "True/false", PasslePost.OpensInNewTabProperty); // TODO: Remove this?

            _contentTypeService.Save(passlePostContentType);
        }

        private void CreatePassleAuthorContentType()
        {
            if (_contentTypeService.Get(_configService.PassleAuthorContentTypeAlias) != null)
            {
                return;
            }

            var passleAuthorContentType = new ContentType(-1)
            {
                Name = "Passle Author",
                Alias = _configService.PassleAuthorContentTypeAlias,
                Description = "A Passle author synced to your Umbraco instance.",
                Icon = "icon-user color-deep-orange",
            };

            passleAuthorContentType.AddPropertyGroup("Content");

            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.NameProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.ShortcodeProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.ProfileUrlProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.AvatarUrlProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.RoleInfoProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textarea", PassleAuthor.DescriptionProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.EmailAddressProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.PhoneNumberProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.LinkedInProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.FacebookProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.TwitterScreenNameProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.XingProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.SkypeProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.VimeoProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.YouTubeProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.StumbleUponProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.PinterestProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.InstagramProfileLinkProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.PersonalLinksProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.LocationDetailProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.LocationCountryProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.TagLineCompanyProperty);
            AddPropertyToContentType(passleAuthorContentType, "Textstring", PassleAuthor.SubscribeLinkProperty);

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
