using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Navigation;
using Mastonet;
using Mastonet.Entities;
using Microsoft.QueryStringDotNET;
using WinMasto.Services;

namespace WinMasto.ViewModels
{
    public class LoginPageViewModel : WinMastoViewModel
    {
        #region Properties
        string _server = string.Empty;
        public string Server { get { return _server; } set { Set(ref _server, value); } }

        private string _serverRedirect = "https://{0}/oauth/authorize/";

        private SettingsService SettingsService = SettingsService.Instance;
        #endregion

        #region Methods

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            IsLoading = false;
        }

        public async Task LoginOauth()
        {
            if (string.IsNullOrEmpty(Server)) return;
            IsLoading = true;
            try
            {
                
                var appRegistration = await GetAppRegistration();
                var client = new MastodonClient(appRegistration);
                var oauthUrl = client.OAuthUrl((string.Format(_serverRedirect, Server)));
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                    WebAuthenticationOptions.None,
                    new Uri(oauthUrl),
                    new System.Uri(string.Format(_serverRedirect, Server)));
                string result;
                string code = "";
                switch (webAuthenticationResult.ResponseStatus)
                {
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                        // Successful authentication. 
                        result = webAuthenticationResult.ResponseData.ToString();
                        var query = QueryString.Parse(result);
                        var testCode = query.FirstOrDefault();
                        code = testCode?.Value;
                        break;
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
                        // HTTP error. 
                        result = webAuthenticationResult.ResponseErrorDetail.ToString();
                        break;
                    default:
                        // Other error.
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                }

                if (string.IsNullOrEmpty(code))
                {
                    // TODO Add error screen
                }
                else
                {
                    var auth = await client.ConnectWithCode(code, string.Format(_serverRedirect, Server));
                    SettingsService.UserAuth = auth;
                    client = new MastodonClient(appRegistration, auth.AccessToken);
                    var account = await client.GetCurrentUser();
                    SettingsService.UserAccount = account;
                    IsLoggedIn = true;

                    if (App.IsTenFoot)
                    {
                        // NavigationService.Navigate(typeof(XboxViews.MainPage));
                    }
                    else
                    {
                        Views.Shell.Instance.ViewModel.IsLoggedIn = true;
                        NavigationService.Navigate(typeof(Views.MainPage));
                    }
                }
            }
            catch (Exception e)
            {
                // TODO: Add error message;
                Console.WriteLine(e);
            }

            //var authUri = $"https://{Server}/oauth/authorize?response_type=code&client_id="
            
            IsLoading = false;
        }

        private async Task<AppRegistration> GetAppRegistration()
        {
            var appRegistration = await MastodonClient.CreateApp(Server, "WinMasto", Scope.Read | Scope.Write | Scope.Follow, null, "https://" + Server + "/oauth/authorize/");
            SettingsService.Instance.AppRegistrationService = appRegistration;
            return appRegistration;
        }

        public async Task LogoutUser()
        {
            
        }

        #endregion
    }
}
