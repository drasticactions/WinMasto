using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Template10.Controls;
using Template10.Services.NavigationService;
using WinMasto.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinMasto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsService _settings;
        public ShellViewModel ViewModel => this.DataContext as ShellViewModel;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            InitilizeTitleBar();
            _settings = Services.SettingsService.Instance;
            ViewModel.CheckLogin();
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
            //HamburgerMenu.IsFullScreen = _settings.IsFullScreen;
            //HamburgerMenu.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
        }

        private void InitilizeTitleBar()
        {
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;

            titleBar.LayoutMetricsChanged += (s, e) =>
            {
                TitleBarGrid.Height = s.Height;
                TitleBarContentGrid.Margin = new Thickness(s.SystemOverlayLeftInset + 12, 0, s.SystemOverlayRightInset - 12, 0);
            };

            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            TitleBarTextBlock.Text = Package.Current.DisplayName;

            view.TitleBar.BackgroundColor = Colors.Transparent;
            view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
