using BasisProjectUmbraco.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NPoco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Infrastructure;
using Umbraco.Cms.Core.Scoping;
using BasisProjectUmbraco.models;
using System;
using Umbraco.Cms.Web.Common.Controllers;
using System.Text.Json;

namespace BasisProjectUmbraco.Controllers
{
    public class CustomSectionController : UmbracoApiController
    {
        private readonly IScopeProvider _scopeProvider;

        public CustomSectionController(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
        //public CustomSectionController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IScopeProvider scopeProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        //{
        //    _scopeProvider = scopeProvider;
        //}

        [HttpPost]
        [Route("CustomSection/Create")]
        public string HandlePostCreateSectionPart(CustomSectionPartViewModel vm)
        {
            var partToAdd = new CustomSectionPart()
            {
                Created = DateTime.Now,
                Title = vm.Title,
                Description = vm.Description
            };
            if (!partToAdd.IsValid())
            {
                return "{\"message\":\"Invalid data.\"}";
            }
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var database = scope.Database;
                database.Insert<CustomSectionPart>(partToAdd);
                scope.Complete();
            }
            return "{\"message\":\"Record has been created!\"}";
        }

        [HttpPost]
        [Route("CustomSection/Update")]
        public string HandlePostUpdateSectionPart(CustomSectionPartViewModel vm)
        {
            var partToUpdate = new CustomSectionPart()
            {
                Id = vm.Id,
                Created = DateTime.Now,
                Title = vm.Title,
                Description = vm.Description
            };
            if (!partToUpdate.IsValid())
            {
                return "{\"message\":\"Invalid data.\"}";
            }
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var database = scope.Database;
                database.Update(partToUpdate);
                scope.Complete();
            }
            return "{\"message\":\"Record has been created!\"}";
        }


        [HttpGet]
        [Route("CustomSection/GetById")]
        public string GetPartById(int id)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var database = scope.Database;
                var query = new Sql()
                    .Select("*")
                    .From("MyCustomSectionParts")
                    .Where("Id=" + id);

                return database.Fetch<CustomSectionPart>(query).Count > 0
                    ? JsonSerializer.Serialize(database.Fetch<CustomSectionPart>(query)[0])
                    : null;
            }
        }

    }
}
