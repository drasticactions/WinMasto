using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using FontAwesome.UWP;

namespace WinMasto.Tools.Converters
{
    public class FavoriteIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var favorited = (bool) value;
                return favorited ? FontAwesomeIcon.Star : FontAwesomeIcon.StarOutline;
            }
            catch (Exception e)
            {
                return FontAwesomeIcon.StarOutline;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
