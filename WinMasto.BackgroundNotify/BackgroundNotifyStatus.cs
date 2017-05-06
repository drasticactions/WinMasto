using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Mastonet;
using Mastonet.Entities;
using Newtonsoft.Json;
using WinMasto.Core.Notifications;

namespace WinMasto.BackgroundNotify
{
    public sealed class BackgroundNotifyStatus : IBackgroundTask
    {
        bool _isLoggedIn;
        private MastodonClient _client;
        readonly Template10.Services.SettingsService.ISettingsHelper _helper;

        public BackgroundNotifyStatus()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        private bool LoginUser()
        {
            var appRegistrationString = _helper.Read<string>("AppRegistrationService", null);
            if (string.IsNullOrEmpty(appRegistrationString)) return false;
            var appRegistration = JsonConvert.DeserializeObject<AppRegistration>(appRegistrationString);
            var userAuthString = _helper.Read<string>("UserAuth", null);
            if (string.IsNullOrEmpty(userAuthString)) return false;
            var userAuth = JsonConvert.DeserializeObject<Auth>(userAuthString);
            var instance = _helper.Read<string>("ServerInstance", null);
            if (appRegistration == null || userAuth == null) return false;
            appRegistration.Instance = instance;
            _client = new MastodonClient(appRegistration, new Auth() { AccessToken = userAuth.AccessToken });
            return true;
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                if (_helper.Read<bool>("BackgroundEnable", false))
                {
                    var result = LoginUser();
                    if (result)
                    {
                        if (_helper.Read<bool>("ToastNotifications", false))
                        {
                            var lastRefreshToast = _helper.Read<DateTime>("LastRefreshToast", DateTime.UtcNow);
                            await CreateNotificationToast(lastRefreshToast);
                            _helper.Write<DateTime>("LastRefreshToast", DateTime.UtcNow);
                        }
                        if (_helper.Read<bool>("BackgroundTile", false))
                        {
                            var lastRefreshTile = _helper.Read<DateTime>("LastRefreshTile", DateTime.UtcNow);
                            await CreateNotificationTiles(lastRefreshTile);
                            await CreateStatusTiles(lastRefreshTile);
                            _helper.Write<DateTime>("LastRefreshTile", DateTime.UtcNow);
                        }
                    }
                }

            }
            catch (Exception)
            {
            }

            deferral.Complete();
        }

        private async Task CreateNotificationToast(DateTime lastRefreshTime)
        {
            var notifications = await _client.GetNotifications();
            var newNotifications = notifications.Where(node => node.CreatedAt > lastRefreshTime);
            foreach (var notification in notifications)
            {
                NotifyStatusTile.CreateToastNotification(notification);
            }
        }

        private async Task CreateNotificationTiles(DateTime lastRefreshTime)
        {
            var notifications = await _client.GetNotifications();
            var newNotifications = notifications.Where(node => node.CreatedAt > lastRefreshTime);
            foreach (var notification in notifications)
            {
                NotifyStatusTile.CreateNotificationLiveTile(notification);
            }
        }

        private async Task CreateStatusTiles(DateTime lastRefreshTime)
        {
            var statusList = await _client.GetHomeTimeline();
            var newStatus = statusList.Where(node => 
                                node.CreatedAt > lastRefreshTime 
                                && (node.Sensitive == null || node.Sensitive.Value == false));
            foreach (var status in newStatus)
            {
                NotifyStatusTile.CreateStatusLiveTile(status);
            }
        }
    }
}
