using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class MainPageViewModel : WinMastoViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            IsLoading = false;
        }

        public async Task NavigateToLoginView()
        {
            await NavigationService.NavigateAsync(typeof(LoginPage));
        }
    }
}
