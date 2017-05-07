using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using WinMasto.Tools;

namespace WinMasto.ViewModels
{
    public class NotificationPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            Title = "Notifications";
            await LoginUser();
            if (IsLoggedIn)
            {

                if (mode == NavigationMode.New || Notifications == null)
                {
                    Notifications = new NotificationScrollingCollection(Client);
                }
                RaisePropertyChanged("Notifications");
            }
            IsLoading = false;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            if (IsLoggedIn)
            {
            }

            return base.OnNavigatingFromAsync(args);
        }
    }
}
