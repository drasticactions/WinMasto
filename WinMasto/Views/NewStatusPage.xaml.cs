using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinMasto.Models;
using WinMasto.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinMasto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewStatusPage : Page
    {
        public NewStatusPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        public NewStatusPageViewModel ViewModel => this.DataContext as NewStatusPageViewModel;

        private async void SelectPhotos_OnClick(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {

                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;
            var photoFileInfo = new PhotoFileInfo()
            {
                File = file,
                Thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem)
            };
            ViewModel.PhotoList.Add(photoFileInfo);
        }

        private void RemovePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as Button;
            var status = menuFlyoutItem?.CommandParameter as PhotoFileInfo;
            if (status == null) return;
            ViewModel.PhotoList.Remove(status);
        }
    }
}
