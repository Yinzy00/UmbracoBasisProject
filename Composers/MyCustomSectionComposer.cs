using BasisProjectUmbraco.NotificationHandler;
using Microsoft.Extensions.Logging;
using NPoco;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace BasisProjectUmbraco.Composers
{
    public class MyCustomSectionComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, MyCustomSectionTableHandler>();
        }
    }

    //public class CustomSectionComponent : IComponent
    //{

    //    private readonly IScopeProvider _scopeProvider;
    //    private readonly IMigrationPlanExecutor _migrationPlanExecutor;
    //    private readonly IKeyValueService _keyValueService;
    //    private readonly IRuntimeState _runtimeState;

    //    public CustomSectionComponent(
    //        IScopeProvider scopeProvider,
    //        IMigrationPlanExecutor migrationPlanExecutor,
    //        IKeyValueService keyValueService,
    //        IRuntimeState runtimeState)
    //    {
    //        _scopeProvider = scopeProvider;
    //        _migrationPlanExecutor = migrationPlanExecutor;
    //        _keyValueService = keyValueService;
    //        _runtimeState = runtimeState;
    //    }

    //    public void Initialize()
    //    {
    //        if (_runtimeState.Level < RuntimeLevel.Run)
    //        {
    //            return;
    //        }

    //        var migrationPlan = new MigrationPlan("CustomSectionPars");

    //        migrationPlan.From(string.Empty).To<AddCustomSectionParts>("customSectionParts-db");

    //        var ugrader = new Upgrader(migrationPlan);
    //        ugrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);

    //    }

    //    public void Terminate()
    //    {
    //    }
    //}

    //public class AddCustomSectionParts : MigrationBase
    //{
    //    public AddCustomSectionParts(IMigrationContext context) : base(context)
    //    {
    //    }

    //    protected override void Migrate()
    //    {
    //        Logger.LogDebug("Running migration {MigrationStep}", "AddcustomSectionPartsTable");

    //        if (!TableExists("CustomSectionParts"))
    //        {
    //            Create.Table<CustomSectionPartSchema>().Do();
    //        }
    //        else
    //        {
    //            Logger.LogDebug("The database table {DbTable} already exists, skipping", "BlogComments");
    //        }
    //    }
    //}

    //[TableName("CustomSectionParts")]
    //[PrimaryKey("Id", AutoIncrement = true)]
    //[ExplicitColumns]
    //public class CustomSectionPartSchema
    //{
    //    [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
    //    [Column("Id")]
    //    public int Id { get; set; }

    //    [Column("Name")]
    //    public string Name { get; set; }

    //    [Column("Created")]
    //    public DateTime Created { get; set; }
    //}
}
