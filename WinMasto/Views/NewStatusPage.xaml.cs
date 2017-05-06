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
            VisibilityListBox.SelectedIndex = 0;
        }

        public NewStatusPageViewModel ViewModel => this.DataContext as NewStatusPageViewModel;

        private async void SelectPhotos_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsLoading = true;
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
            if (file == null)
            {
                ViewModel.IsLoading = false;
                return;
            }
            var photoFileInfo = new PhotoFileInfo()
            {
                File = file,
                Thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem)
            };
            using (var randomStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var attachment = await ViewModel.Client.UploadMedia(randomStream.AsStream(), Path.GetFileName(file.Path));
                try
                {
                    StatusTextBox.Text += $" {attachment.Url}";
                    photoFileInfo.Attachment = attachment;
                    ViewModel.PhotoList.Add(photoFileInfo);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                ViewModel.IsLoading = false;
            }
        }

        private void RemovePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as Button;
            var status = menuFlyoutItem?.CommandParameter as PhotoFileInfo;
            if (status == null) return;
            var newText = StatusTextBox.Text.Remove(StatusTextBox.Text.IndexOf(status.Attachment.Url, StringComparison.Ordinal), status.Attachment.Url.Length);
            StatusTextBox.Text = newText;
            ViewModel.PhotoList.Remove(status);
        }

        private void ChangeSpoilerMode_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Sensitive = !ViewModel.Sensitive;
        }

        private void VisibilityChanged_OnClick(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.StatusVisibility = (Mastonet.Visibility) VisibilityListBox.SelectedIndex;
        }
    }
}
