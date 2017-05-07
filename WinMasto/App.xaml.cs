using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Controls;
using WinMasto.Core.Notifications;
using WinMasto.Services;
using WinMasto.Tools.BackgroundTasks;

namespace WinMasto
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        SettingsService _settingsService = SettingsService.Instance;

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            #region Xbox
            if (IsTenFoot)
            {
                this.RequiresPointerMode = Windows.UI.Xaml.ApplicationRequiresPointerMode.WhenRequested;
            }
            #endregion

            RequestedTheme = IsTenFoot ? ApplicationTheme.Dark : _settingsService.AppTheme;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(330, 200));
            if (IsTenFoot)
            {
                // Turn off overscan. We'll be handling it.
                var AppView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                AppView.SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
            }
            SetupBackgroundServices();
            SetTitleBarColor();
            await SetupStartupLocation(startKind, args);
        }

        private async void SetupBackgroundServices()
        {
            IsIoT = ApiInformation.IsTypePresent("Windows.Devices.Gpio.GpioController");

            if (IsIoT) return;

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.ToastBackgroundTaskName);
            var task2 = await
                BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.ToastBackgroundTaskEntryPoint,
                    BackgroundTaskUtils.ToastBackgroundTaskName, new ToastNotificationActionTrigger(),
                    null);

            if (SettingsService.Instance.BackgroundEnable)
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                var task = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                        BackgroundTaskUtils.BackgroundTaskName,
                        new TimeTrigger(15, false),
                        null);
            }
        }

        private async Task SetupStartupLocation(StartKind startKind, IActivatedEventArgs args)
        {
            //if (IsTenFoot)
            //{
            //    // await NavigationService.NavigateAsync(typeof(XboxViews.MainPage));
            //    return;
            //}
            try
            {
                if (startKind == StartKind.Activate)
                {
                    if (args.Kind == ActivationKind.ToastNotification)
                    {
                        StartupFromToast(args);
                    }
                    return;
                    //if (args.Kind == ActivationKind.VoiceCommand)
                    //    await StartupFromVoice(args);
                    //if (args.Kind == ActivationKind.Protocol)
                    //    StartupFromProtocol(args);
                }
                else
                {
                    await NavigationService.NavigateAsync(typeof(Views.MainPage), "home");
                }
            }
            catch (Exception)
            {
                // If all else fails, go to the main page.
                await NavigationService.NavigateAsync(typeof(Views.MainPage), "home");
            }

            var login = SettingsService.Instance.UserAuth;
            if (login == null)
            {
                await NavigationService.NavigateAsync(typeof(Views.LoginPage));
            }
            else
            {
                await NavigationService.NavigateAsync(typeof(Views.MainPage), "home");
            }
        }

        private async void StartupFromToast(IActivatedEventArgs args)
        {
            var toastArgs = args as ToastNotificationActivatedEventArgs;
            if (toastArgs == null)
                throw new Exception("Not Toast Arguments!");
            var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(toastArgs.Argument);
            switch (arguments.Type)
            {
                case ToastType.Account:
                    await NavigationService.NavigateAsync(typeof(Views.AccountPage), JsonConvert.SerializeObject(arguments.Account));
                    break;
                case ToastType.Status:
                    await NavigationService.NavigateAsync(typeof(Views.StatusPage), arguments.Status);
                    break;
                default:
                    await NavigationService.NavigateAsync(typeof(Views.MainPage), "home");
                    break;
            }
        }

        public override void OnResuming(object s, object e, BootStrapper.AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
            SetTitleBarColor();
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            if (!IsTenFoot)
            {
                var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                return new ModalDialog()
                {
                    ModalContent = new Views.Busy(),
                    Content = new Views.Shell(service)
                };
            }
            else
            {
                var navigationFrame = new Frame();
                var navigationService = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, navigationFrame);
                return new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = navigationFrame
                };
            }
        }

        public void SetTitleBarColor()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            //windows title bar      
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Colors.Transparent;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ForegroundColor = Colors.Transparent;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Colors.Transparent;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Colors.Transparent;

            //StatusBar for Mobile

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = (Color)Application.Current.Resources["SystemAccentColor"];
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }

            if (IsTenFoot)
            {
                Application.Current.Resources["SystemControlHighlightAccentBrush"] = Colors.Black;
            }
        }
        #region iot
        public static bool IsIoT { get; private set; } = false;
        #endregion
        #region Xbox
        public static bool IsTenFootPC { get; private set; } = false;

        public static bool IsTenFoot
        {
            get
            {
                return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox" || Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic" || IsTenFootPC;
            }
            set
            {
                if (value != IsTenFootPC)
                {
                    IsTenFootPC = value;
                    TenFootModeChanged?.Invoke(null, null);
                }
            }
        }

        public static event EventHandler TenFootModeChanged;

        #endregion
    }
}
