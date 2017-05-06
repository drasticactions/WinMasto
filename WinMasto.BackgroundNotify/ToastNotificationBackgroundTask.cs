using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Newtonsoft.Json;
using Template10.Services.SettingsService;
using WinMasto.Windows.Core.Notifications;

namespace WinMasto.BackgroundNotify
{
    public sealed class ToastNotificationBackgroundTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Template10.Services.SettingsService.ISettingsHelper _helper = new SettingsHelper();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            var arguments = details?.Argument;
            if (arguments != null)
            {
                var args = JsonConvert.DeserializeObject<ToastNotificationArgs>(arguments);
                if (args.Type == ToastType.Sleep)
                {
                    _helper.Write("ToastNotifications", false);
                }
            }

            deferral.Complete();
        }
    }
}
