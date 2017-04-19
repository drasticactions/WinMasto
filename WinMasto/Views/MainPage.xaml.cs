using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using WinMasto.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinMasto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

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

        private async void Report_OnClick(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var status = menuFlyoutItem?.CommandParameter as Status;
            await ViewModel.ReportOption(status);
        }

        private async void Favorite_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            await ViewModel.FavoriteOption(status);
        }

        private async void ReShare_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            await ViewModel.ReShareOption(status);
        }

        private async void Reply_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            await ViewModel.ReplyOption(status);
        }

        private void ImageItem_OnClick(object sender, ItemClickEventArgs e)
        {
            var attachment = e.ClickedItem as Attachment;
            if (attachment == null) return;

            var grid = sender as AdaptiveGridView;
            if (grid == null) return;

            var status = grid.DataContext as Status;
            if (status == null) return;
            ImageGalleryView.ViewModel.SetStatus(status, attachment);
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(StatusGrid);
            flyoutBase.ShowAt(StatusGrid);
        }

        private async void ShowNSFWPost_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var status = button?.CommandParameter as Status;
            await ViewModel.ShowNSFWPost(status);
        }
    }
}
