using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mastonet;
using Mastonet.Entities;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class NewStatusPageViewModel : WinMastoViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            IsLoading = false;
        }

        private string _status = "";

        public string Status
        {
            get { return _status; }
            set
            {
                Set(ref _status, value);
            }
        }

        private int _statusCount = 500;

        public int StatusCount
        {
            get { return _statusCount; }
            set
            {
                Set(ref _statusCount, value);
            }
        }

        public void StatusTextBox_OnChanged(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            StatusCount = 500 - sender.Text.Length;
        }

        public async Task SendStatus()
        {
            // TODO: This is for testing the postStatus function. Make this more generic.
            if (string.IsNullOrEmpty(Status) || Status.Length > 500) return;
            var result = await Client.PostStatus(Status, Visibility.Public);
            await NavigationService.NavigateAsync(typeof(MainPage));
        }
    }
}
