using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Mastonet.Entities;
using Newtonsoft.Json;
using Template10.Mvvm;
using WinMasto.Services;
using WinMasto.Tools;

namespace WinMasto.ViewModels
{
    public class AccountPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            Title = "";
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
                            try
                            {
                                var relationships = await Client.GetAccountRelationships(testAccount.Id);
                                if (relationships.Any())
                                {
                                    Relationship = relationships.FirstOrDefault();
                                }
                            }
                            catch (Exception e)
                            {
                                Relationship = new Relationship();
                            }
                            getNewStatus = true;
                            IsCurrentUser = false;
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
                            IsCurrentUser = true;
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

        public async Task SetFollowField()
        {
            try
            {
                if (Relationship.Following)
                {
                    Relationship = await Client.Unfollow(Account.Id);
                }
                else
                {
                    Relationship = await Client.Follow(Account.Id);
                }
            }
            catch (Exception e)
            {
                // TODO: Add error handling
            }
        }

        public async Task SetBlockField()
        {
            try
            {
                if (Relationship.Blocking)
                {
                    Relationship = await Client.Unblock(Account.Id);
                }
                else
                {
                    Relationship = await Client.Block(Account.Id);
                }
            }
            catch (Exception e)
            {
                // TODO: Add error handling
            }
        }

        public async Task SetMuteField()
        {
            try
            {
                if (Relationship.Muting)
                {
                    Relationship = await Client.Unmute(Account.Id);
                }
                else
                {
                    Relationship = await Client.Mute(Account.Id);
                }
            }
            catch (Exception e)
            {
                // TODO: Add error handling
            }
        }

        private Relationship _relationship;

        public Relationship Relationship
        {
            get { return _relationship; }
            set
            {
                Set(ref _relationship, value);
            }
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

        private bool _isCurrentUser;

        public bool IsCurrentUser
        {
            get { return _isCurrentUser; }
            set
            {
                Set(ref _isCurrentUser, value);
            }
        }
    }
}
