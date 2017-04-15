using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet;
using Mastonet.Entities;
using Template10.Mvvm;
using WinMasto.Services;

namespace WinMasto.ViewModels
{
    public class WinMastoViewModel : ViewModelBase
    {
        public MastodonClient Client { get; set; }

        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => Set(ref _isLoggedIn, value);
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        #region User

        private Account _userAccount;

        public Account UserAccount
        {
            get => _userAccount;
            set => Set(ref _userAccount, value);
        }

        #endregion

        public async Task LoginUser()
        {
            var appRegistration = SettingsService.Instance.AppRegistrationService;
            var userAuth = SettingsService.Instance.UserAuth;
            if (appRegistration == null || userAuth == null) return;
            Client = new MastodonClient(appRegistration, userAuth.AccessToken);
            UserAccount = SettingsService.Instance.UserAccount;
            IsLoggedIn = true;
        }
    }
}
