using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mastonet;
using Mastonet.Entities;
using Template10.Services.NavigationService;
using Template10.Utils;
using WinMasto.Tools;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class MainPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            var path = (string)parameter;
            SetTitle(path);
            await LoginUser();
            if (IsLoggedIn)
            {
               
                Statuses = new TimelineScrollingCollection(Client, path);
                RaisePropertyChanged("Statuses");
            }
            IsLoading = false;
        }

        private void SetTitle(string path)
        {
            switch (path)
            {
                case "home":
                    Title = "Home Timeline";
                    break;
                case "public":
                    Title = "Federated Timeline";
                    break;
                case "local":
                    Title = "Local Timeline";
                    break;
                default:
                    Title = "Home Timeline";
                    break;
            }
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
