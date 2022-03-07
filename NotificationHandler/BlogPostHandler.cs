using System;
using System.Linq;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace BasisProjectUmbraco.NotificationHandler
{
    public class BlogPostHandler : INotificationHandler<SendingContentNotification>
    {
        public void Handle(SendingContentNotification notification)
        {
            if (notification.Content.ContentTypeAlias == "blogPost")
            {
                foreach (var variant in notification.Content.Variants)
                {
                    if (variant.State == ContentSavedState.NotCreated)
                    {
                        var pubDateProperty = variant.Tabs.SelectMany(f => f.Properties)
                            .FirstOrDefault(f => f.Alias.InvariantEquals("posted"));
                        if (pubDateProperty is not null)
                        {
                            pubDateProperty.Value = DateTime.UtcNow;
                        }
                    }
                }
            }
        }
    }
}
