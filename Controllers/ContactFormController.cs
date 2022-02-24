using BasisProjectUmbraco.Handlers;
using BasisProjectUmbraco.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace BasisProjectUmbraco.Controllers
{
    public class ContactFormController : SurfaceController
    {
        private readonly ISmtpHandler _smtpHandler;
        public ContactFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ISmtpHandler smtpHandler) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _smtpHandler = smtpHandler;
        }

        [HttpPost]
        public IActionResult SubmitForm(ContactFormViewModel model)
        {
            var body = model.FirstName + " " + model.LastName + Environment.NewLine + Environment.NewLine + model.Bericht;

            _smtpHandler.Send(body, "testing@yarimarien.be", "testing@yarimarien.be", "Contacto form data");
            //sh.SendMail();
            return RedirectToCurrentUmbracoPage();
        }
    }
}
