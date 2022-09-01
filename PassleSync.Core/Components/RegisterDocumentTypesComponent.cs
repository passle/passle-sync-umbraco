using PassleSync.Core.Attributes;
using PassleSync.Core.Constants;
using PassleSync.Core.Extensions;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
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

        private int _passleContainerId;
        private int _elementsContainerId;

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
            CreateContentTypeFolders();
            CreatePasslePostContentType();
            CreatePassleAuthorContentType();
        }

        private void CreateDataTypes()
        {
            if (_dataTypeService.GetDataType(PassleDataType.PASSLE_REPEATABLE_TEXTSTRINGS) == null)
            {
                var editor = Current.Factory.GetInstance<MultipleTextStringPropertyEditor>();
                var dataType = new DataType(editor)
                {
                    Name = PassleDataType.PASSLE_REPEATABLE_TEXTSTRINGS,
                };

                _dataTypeService.Save(dataType);
            }

            if (_dataTypeService.GetDataType(PassleDataType.PASSLE_LABEL_LONG_STRING) == null)
            {
                var editor = Current.Factory.GetInstance<LabelPropertyEditor>();
                var dataType = new DataType(editor)
                {
                    Name = PassleDataType.PASSLE_LABEL_LONG_STRING,
                    Configuration = new LabelConfiguration()
                    {
                        ValueType = ValueTypes.Text,
                    },
                };

                _dataTypeService.Save(dataType);
            }
        }

        private void CreateContentTypeFolders()
        {
            var passleContainerAttempt = _contentTypeService.CreateContainer(-1, "Passle");
            if (passleContainerAttempt.Success)
            {
                _passleContainerId = passleContainerAttempt.Result.Entity.Id;
            }
            else
            {
                _passleContainerId = _contentTypeService.GetContainers("Passle", 1).First().Id;
            }

            var elementsContainerAttempt = _contentTypeService.CreateContainer(_passleContainerId, "Elements");
            if (elementsContainerAttempt.Success)
            {
                _elementsContainerId = elementsContainerAttempt.Result.Entity.Id;
            }
            else
            {
                _elementsContainerId = _contentTypeService.GetContainers("Elements", 2).First().Id;
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
                ParentId = _passleContainerId,
            };

            AddAllPropertiesOfTypeToContentType(passlePostContentType, typeof(PasslePost));

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
                ParentId = _passleContainerId,
            };

            AddAllPropertiesOfTypeToContentType(passleAuthorContentType, typeof(PassleAuthor));

            _contentTypeService.Save(passleAuthorContentType);
        }

        private string CreateElementTypeForType(Type type)
        {
            var alias = type.Name.ToPropertyAlias();

            if (_contentTypeService.Get(alias) != null)
            {
                return alias;
            }

            var contentType = new ContentType(-1)
            {
                Name = type.Name.FromPascalCaseToTitleCase(),
                Alias = alias,
                Icon = "icon-science color-deep-orange",
                IsElement = true,
                ParentId = _elementsContainerId,
            };

            AddAllPropertiesOfTypeToContentType(contentType, type);

            _contentTypeService.Save(contentType);

            return alias;
        }

        private string CreateNestedContentDataType(string elementTypeAlias, string nameTemplate = null)
        {
            var name = $"Passle Nested Content - {elementTypeAlias.FirstCharToUpper()}";

            if (_dataTypeService.GetDataType(name) != null)
            {
                return name;
            }

            var contentType = new NestedContentConfiguration.ContentType()
            {
                Alias = elementTypeAlias,
                TabAlias = "Content",
            };

            if (nameTemplate != null)
            {
                contentType.Template = nameTemplate;
            }

            var editor = Current.Factory.GetInstance<NestedContentPropertyEditor>();
            var dataType = new DataType(editor)
            {
                Name = name,
                Configuration = new NestedContentConfiguration()
                {
                    ContentTypes = new[]
                    {
                        contentType,
                    },
                },
            };

            _dataTypeService.Save(dataType);

            return name;
        }

        private void AddClassTypeToContentType(ContentType contentType, Type type, PropertyInfo property)
        {
            var elementTypeAlias = CreateElementTypeForType(type);
            string dataTypeName;
            
            var nestedContentNameTemplateAttribute = type.GetCustomAttribute<NestedContentNameTemplateAttribute>();
            if (nestedContentNameTemplateAttribute != null)
            {
                var template = nestedContentNameTemplateAttribute.Template;
                dataTypeName = CreateNestedContentDataType(elementTypeAlias, template);
            }
            else
            {
                dataTypeName = CreateNestedContentDataType(elementTypeAlias);
            }

            AddPropertyToContentType(contentType, dataTypeName, property.Name);
        }

        private void AddAllPropertiesOfTypeToContentType(ContentType contentType, Type type)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var propertyTypeInfo = property.PropertyType;
                var isEnumerable = false;

                if (propertyTypeInfo.Implements<IEnumerable>() && propertyTypeInfo.IsGenericType)
                {
                    propertyTypeInfo = propertyTypeInfo.GetGenericArguments()[0];
                    isEnumerable = true;
                }
                else if (!propertyTypeInfo.IsSimpleType())
                {
                    continue;
                }

                if (propertyTypeInfo.IsSerializable)
                {
                    string dataTypeName = PassleDataType.LABEL_STRING;

                    if (propertyTypeInfo == typeof(bool))
                    {
                        dataTypeName = PassleDataType.BOOLEAN;
                    }
                    else if (propertyTypeInfo == typeof(int) || propertyTypeInfo == typeof(int?))
                    {
                        dataTypeName = PassleDataType.LABEL_INTEGER;
                    }

                    if (isEnumerable)
                    {
                        dataTypeName = PassleDataType.PASSLE_REPEATABLE_TEXTSTRINGS;
                    }
                    else if (property.IsDefined(typeof(LongStringAttribute), false))
                    {
                        dataTypeName = PassleDataType.PASSLE_LABEL_LONG_STRING;
                    }

                    AddPropertyToContentType(contentType, dataTypeName, property.Name);
                }
                else
                {
                    AddClassTypeToContentType(contentType, propertyTypeInfo, property);
                }
            }
        }

        private void AddPropertyToContentType(ContentType contentType, string dataTypeName, string propertyName)
        {
            var dataType = _dataTypeService.GetDataType(dataTypeName);
            var propertyType = new PropertyType(dataType, propertyName.ToPropertyAlias())
            {
                Name = propertyName,
            };

            contentType.AddPropertyType(propertyType, "Content");
        }
    }
}
