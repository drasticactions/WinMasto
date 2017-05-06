using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;

namespace WinMasto.Windows.Core.Notifications
{
    public class ToastNotificationArgs
    {
        public ToastType Type { get; set; }

        public int AccountId { get; set; }

        public int StatusId { get; set; }
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
