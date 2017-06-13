using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Mastonet.Entities;
using Newtonsoft.Json;
using Template10.Utils;
using WinMasto.Tools.BackgroundTasks;
using Application = Windows.UI.Xaml.Application;

namespace WinMasto.Services
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public AppRegistration AppRegistrationService
        {
            get
            {
                var app = _helper.Read<string>(nameof(AppRegistrationService), null);
                if (string.IsNullOrEmpty(app)) return null;
                return JsonConvert.DeserializeObject<AppRegistration>(app);
            }
            set
            {
                _helper.Write(nameof(AppRegistrationService), JsonConvert.SerializeObject(value));
            }
        }

        public Account UserAccount
        {
            get
            {
                var app = _helper.Read<string>(nameof(UserAccount), null);
                if (string.IsNullOrEmpty(app)) return null;
                return JsonConvert.DeserializeObject<Account>(app);
            }
            set
            {
                _helper.Write(nameof(UserAccount), JsonConvert.SerializeObject(value));
            }
        }

        public Auth UserAuth
        {
            get
            {
                var app = _helper.Read<string>(nameof(UserAuth), null);
                if (string.IsNullOrEmpty(app)) return null;
                return JsonConvert.DeserializeObject<Auth>(app);
            }
            set
            {
                _helper.Write(nameof(UserAuth), JsonConvert.SerializeObject(value));
            }
        }

        public string ServerInstance
        {
            get { return _helper.Read<string>(nameof(ServerInstance), ""); }
            set
            {
                _helper.Write(nameof(ServerInstance), value);
            }
        }

        public bool BackgroundEnable
        {
            get { return _helper.Read<bool>(nameof(BackgroundEnable), false); }
            set
            {
                _helper.Write(nameof(BackgroundEnable), value);
                ChangeBackgroundStatus(value);
            }
        }

        public bool BackgroundTile
        {
            get { return _helper.Read<bool>(nameof(BackgroundTile), false); }
            set
            {
                _helper.Write(nameof(BackgroundTile), value);
            }
        }

        public bool AlwaysShowNSFW
        {
            get { return _helper.Read<bool>(nameof(AlwaysShowNSFW), false); }
            set
            {
                _helper.Write(nameof(AlwaysShowNSFW), value);
            }
        }

        public bool ToastNotifications
        {
            get { return _helper.Read<bool>(nameof(ToastNotifications), false); }
            set
            {
                _helper.Write(nameof(ToastNotifications), value);
            }
        }

        public DateTime LastRefreshToast
        {
            get { return _helper.Read<DateTime>(nameof(LastRefreshToast), DateTime.UtcNow); }
            set
            {
                _helper.Write(nameof(LastRefreshToast), value);
            }
        }

        public DateTime LastRefreshTile
        {
            get { return _helper.Read<DateTime>(nameof(LastRefreshTile), DateTime.UtcNow); }
            set
            {
                _helper.Write(nameof(LastRefreshTile), value);
            }
        }

        public async void ChangeBackgroundStatus(bool value)
        {
            if (value)
            {
                var task = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                        BackgroundTaskUtils.BackgroundTaskName,
                        new TimeTrigger(15, false),
                        null);
            }
            else
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
            }
        }

        public delegate void ChangedAppTheme();

        public event ChangedAppTheme ChangedAppThemeHandler;

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = Application.Current.RequestedTheme;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                //(Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                //Views.Shell.HamburgerMenu.RefreshStyles(value, true);
                //if (ChangedAppThemeHandler != null) ChangedAppThemeHandler.Invoke();
            }
        }

        public bool IsFullScreen
        {
            get { return _helper.Read<bool>(nameof(IsFullScreen), false); }
            set
            {
                _helper.Write(nameof(IsFullScreen), value);
                // Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }
    }
}
