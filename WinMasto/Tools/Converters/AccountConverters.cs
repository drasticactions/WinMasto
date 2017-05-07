using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using FontAwesome.UWP;
using Mastonet.Entities;

namespace WinMasto.Tools.Converters
{
    public class IsFollowingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var relationship = value as Relationship;
            if (relationship == null) return null;
            return relationship.Following ? FontAwesomeIcon.MinusCircle : FontAwesomeIcon.PlusCircle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IsMutedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var relationship = value as Relationship;
            if (relationship == null) return null;
            return relationship.Muting ? FontAwesomeIcon.Microphone : FontAwesomeIcon.MicrophoneSlash;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IsBlockingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var relationship = value as Relationship;
            if (relationship == null) return null;
            return relationship.Blocking ? FontAwesomeIcon.Unlock : FontAwesomeIcon.Lock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
