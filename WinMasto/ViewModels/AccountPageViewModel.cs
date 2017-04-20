using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Mastonet.Entities;
using Newtonsoft.Json;
using WinMasto.Services;
using WinMasto.Tools;

namespace WinMasto.ViewModels
{
    public class AccountPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            Title = "Account";
            await LoginUser();
            if (IsLoggedIn)
            {
                try
                {
                    if (parameter != null)
                    {
                        var testAccount = JsonConvert.DeserializeObject<Account>((string) parameter);
                        Account = await Client.GetAccount(testAccount.Id);
                    }
                    else
                    {
                        Account = SettingsService.Instance.UserAccount;
                    }
                }
                catch (Exception e)
                {
                    // TODO: Show an error if we can't show the user account.
                    Account = SettingsService.Instance.UserAccount;
                }
                Title = Account.UserName;
                Statuses = new TimelineScrollingCollection(Client, "account", Account.Id);
                RaisePropertyChanged("Statuses");
            }
            IsLoading = false;
        }


        private Account _account;

        public Account Account
        {
            get { return _account; }
            set
            {
                Set(ref _account, value);
            }
        }
    }
}
