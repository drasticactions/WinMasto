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
            await LoginUser();
            var getNewStatus = false;
            // TODO: The navigation cache will make it so that it will always get a new page.
            // I need to figure out a better way to handle this. This is a mess.
            if (IsLoggedIn)
            {
                try
                {
                    if (parameter != null)
                    {
                        var testAccount = JsonConvert.DeserializeObject<Account>((string) parameter);
                        if (Account == null || testAccount.Id != Account.Id)
                        {
                            Account = await Client.GetAccount(testAccount.Id);
                            getNewStatus = true;
                        }
                    }
                    else
                    {
                        var serviceAccountTest = SettingsService.Instance.UserAccount;
                        if (Account == null || serviceAccountTest.Id != Account.Id)
                        {
                            Account = await Client.GetAccount(serviceAccountTest.Id);
                            SettingsService.Instance.UserAccount = Account;
                            getNewStatus = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    // TODO: Show an error if we can't show the user account.
                    Account = SettingsService.Instance.UserAccount;
                }
                Title = Account.UserName;
                if (getNewStatus)
                {
                    Statuses = new TimelineScrollingCollection(Client, "account", Account.Id);
                }
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
