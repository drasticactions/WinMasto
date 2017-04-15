using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Template10.Utils;
using WinMasto.BackgroundTasks;

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

        public bool BackgroundEnable
        {
            get { return _helper.Read<bool>(nameof(BackgroundEnable), false); }
            set
            {
                _helper.Write(nameof(BackgroundEnable), value);
                ChangeBackgroundStatus(value);
            }
        }

        public DateTime LastRefresh
        {
            get { return _helper.Read<DateTime>(nameof(LastRefresh), DateTime.Now); }
            set
            {
                _helper.Write(nameof(LastRefresh), value);
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
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                // Views.Shell.HamburgerMenu.RefreshStyles(value, true);
                if (ChangedAppThemeHandler != null) ChangedAppThemeHandler.Invoke();
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
