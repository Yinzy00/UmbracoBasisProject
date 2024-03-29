﻿using BasisProjectUmbraco.NotificationHandler;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace BasisProjectUmbraco.Composers
{
    public class NotificationHandlerComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<SendingContentNotification, BlogPostHandler>();
        }
    }
}
