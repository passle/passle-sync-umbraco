﻿using PassleSync.Core.Services;
using System;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace PassleSync.Core.Components
{
    public class GenerateAPIKeyComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public GenerateAPIKeyComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            // Create a migration plan for a specific project/feature
            var migrationPlan = new MigrationPlan("GenerateAPIKey");

            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty).To<GenerateAPIKey>("GenerateAPIKey");

            // Upgrade the site based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
            //
        }
    }

    public class GenerateAPIKey : MigrationBase
    {
        private readonly ConfigService _configService;

        public GenerateAPIKey(
            IMigrationContext context,
            ConfigService configService) : base(context)
        {
            _configService = configService;
        }

        public override void Migrate()
        {
            Logger.Debug<RegisterDocumentTypes>("Running migration {MigrationStep}", "GenerateAPIKey");

            CreateAPIKey();
        }

        private void CreateAPIKey()
        {
            if (string.IsNullOrEmpty(_configService.PluginApiKey))
            {
                _configService.PluginApiKey = Guid.NewGuid().ToString();
            }
        }
    }
}
