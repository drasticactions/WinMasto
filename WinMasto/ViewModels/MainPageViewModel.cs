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
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class MainPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            if (IsLoggedIn)
            {
                // TODO: Fix core library to allow max_id, since_id
                // TODO: Seperate this into new class, for doing scrolling lists
                Statuses = new ObservableCollection<Status>();
                IEnumerable<Status> statuses = new List<Status>();
                var path = (string) parameter;
                switch (path)
                {
                    case "home":
                        statuses = await Client.GetHomeTimeline();
                        break;
                    case "public":
                        statuses = await Client.GetPublicTimeline();
                        break;
                    case "local":
                        statuses = await Client.GetPublicTimeline(true);
                        break;
                    default:
                        statuses = await Client.GetHomeTimeline();
                        break;
                }
                Statuses.AddRange(statuses);
                RaisePropertyChanged("Statuses");
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
