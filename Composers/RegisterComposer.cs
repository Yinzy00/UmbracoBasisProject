using BasisProjectUmbraco.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace BasisProjectUmbraco.Composers
{
    public class RegisterComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddScoped<ISmtpHandler, SmtpHandler>();
        }
    }
}
