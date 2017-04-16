using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private string _status;

        public string Status { get => _status;
            set => Set(ref _status, value);
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
