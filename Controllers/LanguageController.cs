using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;

namespace BasisProjectUmbraco.Controllers
{
    public class LanguageController : SurfaceController
    {
        public LanguageController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult Change(string language, int modelId)
        {
            return View();
            //CurrentPage.Id;
            ////var pathParts = path.Split('/');
            ////var newPath = "";
            ////for (int i = 1; i < pathParts.Length - 2; i++)
            ////{
            ////    if (!string.IsNullOrEmpty(pathParts[i])) {
            ////        newPath += "/" + pathParts[i];
            ////    }
            ////}
            //HomePage rootModel = (Model.AncestorOrSelf<HomePage>());
            //return Redirect(model.Cultures + "/" + language + "/");
        }
    }
}
