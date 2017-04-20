using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinMasto.Tools.Converters
{
    public class ThumbnailToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage image = null;
            if (value != null)
            {
                if (value.GetType() != typeof(StorageItemThumbnail))
                {
                    throw new ArgumentException("Expected a thumbnail");
                }
                StorageItemThumbnail thumbnail = (StorageItemThumbnail)value;
                image = new BitmapImage();
                image.SetSource(thumbnail);
            }
            return (image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
