using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Mastonet.Entities;

namespace WinMasto.Tools.Converters
{
    public class FollowYouConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var notification = value as Notification;
            if (notification == null) return "";
            return $"@{notification.Account.AccountName} followed you";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
