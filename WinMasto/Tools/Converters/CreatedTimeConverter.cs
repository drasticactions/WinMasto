using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using PrettyPrintNet;

namespace WinMasto.Tools.Converters
{
    public class CreatedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var accountDateTime = (DateTime) value;
            var timespan = DateTime.Now.Subtract(accountDateTime);
            return timespan.ToPrettyString(2, UnitStringRepresentation.Compact);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
