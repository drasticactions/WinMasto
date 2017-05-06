using Mastonet.Entities;

namespace WinMasto.Core.Notifications
{
    public class ToastNotificationArgs
    {
        public ToastType Type { get; set; }

        public Account Account { get; set; }

        public Status Status { get; set; }
    }

    public enum ToastType
    {
        Other,
        Ignore,
        Sleep,
        Account,
        Status
    }
}
