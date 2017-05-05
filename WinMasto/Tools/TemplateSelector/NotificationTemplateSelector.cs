using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Mastonet.Entities;

namespace WinMasto.Tools.TemplateSelector
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {

        public DataTemplate ReblogNotificationDataTemplate { get; set; }

        public DataTemplate MentionNotificationDataTemplate { get; set; }

        public DataTemplate FavouriteNotificationDataTemplate { get; set; }

        public DataTemplate FollowNotificationDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
            DependencyObject container)
        {
            var notification = item as Notification;
            if (notification == null) return MentionNotificationDataTemplate;

            switch (notification.Type)
            {
                case "mention":
                    return MentionNotificationDataTemplate;
                case "reblog":
                    return ReblogNotificationDataTemplate;
                case "favourite":
                    return FavouriteNotificationDataTemplate;
                case "follow":
                    return FollowNotificationDataTemplate;
                default:
                    return MentionNotificationDataTemplate;
            }
        }
    }
}
