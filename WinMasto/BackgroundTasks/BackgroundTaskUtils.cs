using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace WinMasto.BackgroundTasks
{
    public class BackgroundTaskUtils
    {
        public const string BackgroundTaskEntryPoint = "WinMasto.BackgroundNotify.BackgroundNotifyStatus";
        public const string BackgroundTaskName = "BackgroundNotifyStatus";
        public const string ToastBackgroundTaskEntryPoint = "WinMasto.BackgroundNotify.ToastNotificationBackgroundTask";
        public const string ToastBackgroundTaskName = "ToastNotificationBackgroundTask";
        public static string BackgroundTaskProgress = string.Empty;
        public static bool BackgroundTaskRegistered = false;

        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string taskEntryPoint, string name,
            IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var builder = new BackgroundTaskBuilder { Name = name, TaskEntryPoint = taskEntryPoint };

            builder.SetTrigger(trigger);


            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskRegistration task = builder.Register();
            return task;
        }

        public static void UnregisterBackgroundTasks(string name)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                }
            }

            UpdateBackgroundTaskStatus(name, false);
        }

        public static void UpdateBackgroundTaskStatus(string name, bool registered)
        {
            switch (name)
            {
                case BackgroundTaskName:
                    BackgroundTaskRegistered = registered;
                    break;
            }
        }

        public static string GetBackgroundTaskStatus(string name)
        {
            bool registered = false;
            switch (name)
            {
                case BackgroundTaskName:
                    registered = BackgroundTaskRegistered;
                    break;
            }

            string status = registered ? "Registered" : "Unregistered";

            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(name))
            {
                status += " - " + settings.Values[name];
            }

            return status;
        }
    }
}
