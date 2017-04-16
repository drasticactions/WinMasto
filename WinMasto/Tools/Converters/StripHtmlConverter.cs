using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using WinMasto.Tools.Extensions;

namespace WinMasto.Tools.Converters
{
    public class StripHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var accountNameValue = value as string;
            return accountNameValue == null ? value : HtmlRemoval.StripTagsCharArray(WebUtility.HtmlDecode(accountNameValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
