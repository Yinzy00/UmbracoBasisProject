using BasisProjectUmbraco.Composers;
using BasisProjectUmbraco.models;
using Microsoft.Extensions.Logging;
using NPoco;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace BasisProjectUmbraco.NotificationHandler
{
    public class MyCustomSectionTableHandler : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public MyCustomSectionTableHandler(
            IScopeProvider scopeProvider,
            IMigrationPlanExecutor migrationPlanExecutor,
            IKeyValueService keyValueService,
            IRuntimeState runtimeState)
        {
            _migrationPlanExecutor = migrationPlanExecutor;
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            var migrationPlan = new MigrationPlan("MyCustomSectionParts");

            migrationPlan.From(string.Empty).To<AddCustomSectionParts>("MyCustomSectionParts-db");

            var ugrader = new Upgrader(migrationPlan);
            ugrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
        }
    }

    public class AddCustomSectionParts : MigrationBase
    {
        public AddCustomSectionParts(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            Logger.LogDebug("Running migration {MigrationStep}", "AddcustomSectionPartsTable");

            if (!TableExists("MyCustomSectionParts"))
            {
                Create.Table<CustomSectionPart>().Do();
            }
            else
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", "BlogComments");
            }
        }
    }
}
