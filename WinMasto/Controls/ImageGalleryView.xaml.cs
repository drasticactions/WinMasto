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
using WinMasto.Tools;
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

        private async void ShowAccount_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuFlyoutItem;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            if (status == null) return;
            await ViewModel.NavigateToAccountPage(status.Account);
        }

        private async void Favorite_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            ViewModel.Status = await ViewModel.FavoriteOption(status);
        }

        private async void ReShare_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            ViewModel.Status = await ViewModel.ReShareOption(status);
        }

        private async void Reply_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            await ViewModel.ReplyOption(status);
        }


        private async void Mention_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var status = menuFlyoutItem?.CommandParameter as Status;
            await ViewModel.MentionOption(status);
        }

        private async void Mute_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var status = menuFlyoutItem?.CommandParameter as Status;
            await ViewModel.MuteOption(status);
        }

        private async void Block_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var status = menuFlyoutItem?.CommandParameter as Status;
            await ViewModel.BlockOption(status);
        }

        public ImageGalleryViewModel ViewModel => this.DataContext as ImageGalleryViewModel;

        private async void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var error = "";
            try
            {
                var imageSource = sender as Button;
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
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                await MessageDialogMaker.SendMessageDialogAsync("Saved the picture to your camera roll!", true);
            }
            else
            {
                await MessageDialogMaker.SendMessageDialogAsync(error, false);
            }
        }
    }
}
