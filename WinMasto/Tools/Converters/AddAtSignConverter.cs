using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinMasto.Tools.Converters
{
    public class AddAtSignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var accountNameValue = value as string;
            return accountNameValue == null ? "@error" : $"@{accountNameValue}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
