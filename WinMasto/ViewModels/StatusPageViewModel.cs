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
            SelectedStatus = status;
            Others = new ObservableCollection<Status>();
            Title = status.Account.AccountName;
            Card = await Client.GetStatusCard(status.Id);
            HasCard = Card.Title != null;
            var context = await Client.GetStatusContext(status.Id);
            foreach (var item in context.Ancestors.Reverse())
            {
                Others.Insert(0, item);
            }
            foreach (var item in context.Descendants)
            {
                Others.Add(item);
            }
            IsLoading = false;
        }

        private bool _hasCard;

        public bool HasCard
        {
            get { return _hasCard; }
            set
            {
                Set(ref _hasCard, value);
            }
        }

        private Card _card;

        public Card Card
        {
            get { return _card; }
            set
            {
                Set(ref _card, value);
            }
        }

        private ObservableCollection<Status> _others;

        public ObservableCollection<Status> Others
        {
            get { return _others; }
            set
            {
                Set(ref _others, value);
            }
        }

        private Status _selectedStatus;

        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                Set(ref _selectedStatus, value);
            }
        }

    }
}
