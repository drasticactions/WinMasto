using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Mastonet.Entities;
using WinMasto.Tools.Files;
using WinMasto.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinMasto.Controls
{
    public sealed partial class ImageGalleryView : UserControl
    {
        public ImageGalleryView()
        {
            this.InitializeComponent();
        }

        public ImageGalleryViewModel ViewModel => this.DataContext as ImageGalleryViewModel;

        private async void Save_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var imageSource = sender as MenuFlyoutItem;
                var attachment = imageSource?.CommandParameter as Attachment;
                if (attachment == null)
                    return;
                var fileName = Path.GetFileName(new Uri(attachment.Url).AbsolutePath);
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(attachment.Url);
                await FileAccessCommands.SaveStreamToCameraRoll(stream, fileName);
                // TODO: Setup alert system to say photo saved to camera roll.
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
