using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Mastonet.Entities;

namespace WinMasto.Tools.Converters
{
    public class SpoilerOrNSFWVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Services.SettingsService.Instance.AlwaysShowNSFW) return Visibility.Collapsed;
            var status = value as Status;
            if (status == null) return Visibility.Collapsed;
            return (status.Sensitive != null && status.Sensitive.Value) || !string.IsNullOrEmpty(status.SpoilerText) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SpoilerOrNSFWTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as Status;
            if (status == null) return string.Empty;
            return !string.IsNullOrEmpty(status.SpoilerText) ? status.SpoilerText : "NSFW Post";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SpoilerOrNSFWConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as Status;
            if (status == null) return 0;
            if (Services.SettingsService.Instance.AlwaysShowNSFW) return 0;
            return (status.Sensitive != null && status.Sensitive.Value) || !string.IsNullOrEmpty(status.SpoilerText) ? 20 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SpoilerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as Status;
            if (status == null) return 0;
            return !string.IsNullOrEmpty(status.SpoilerText) ? 4 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NSFWConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as Status;
            if (status == null) return 0;
            return (status.Sensitive != null && status.Sensitive.Value) ? 20 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
