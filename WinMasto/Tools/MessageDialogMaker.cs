using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace WinMasto.Tools
{
    public static class MessageDialogMaker
    {
        public static async Task SendMessageDialogAsync(string message, bool isSuccess)
        {
            var title = isSuccess ? "WinMasto: " : "WinMasto - Error: ";
            var dialog = new MessageDialog((string.Concat(title, Environment.NewLine, Environment.NewLine, message)));
            await dialog.ShowAsync();
        }
    }
}
