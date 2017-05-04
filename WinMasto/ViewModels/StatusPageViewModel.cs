using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Mastonet.Entities;

namespace WinMasto.ViewModels
{
    public class StatusPageViewModel : StatusBaseViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode,
            IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            var status = parameter as Status;
            if (status == null)
            {
                IsLoading = false;
                return;
            }
            SelectedStatus = new ObservableCollection<Status> {status};
            Title = status.Account.AccountName;
            var context = await Client.GetStatusContext(status.Id);
            foreach (var item in context.Ancestors.Reverse())
            {
                SelectedStatus.Insert(0, item);
            }
            foreach (var item in context.Descendants)
            {
                SelectedStatus.Add(item);
            }
            IsLoading = false;
        }

        private ObservableCollection<Status> _selectedStatus;

        public ObservableCollection<Status> SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                Set(ref _selectedStatus, value);
            }
        }

    }
}
