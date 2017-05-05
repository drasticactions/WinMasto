using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Mastonet;
using Mastonet.Entities;
using Template10.Utils;

namespace WinMasto.Tools
{
    public class NotificationScrollingCollection : ObservableCollection<Notification>, ISupportIncrementalLoading
    {
        public NotificationScrollingCollection(MastodonClient client)
        {
            HasMoreItems = true;
            IsLoading = false;
            _client = client;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task PullToRefresh()
        {
            var statuses = await RefreshTimeline(true);
            if (statuses.Any())
            {
                var reverseStatus = statuses.Reverse();
                foreach (var status in reverseStatus)
                {
                    this.Insert(0, status);
                }
            }
        }

        public async Task<IEnumerable<Notification>> RefreshTimeline(bool pullToRefresh)
        {
            IsLoading = true;
            IEnumerable<Notification> statuses;
            try
            {
                ArrayOptions options = new ArrayOptions();
                if (pullToRefresh)
                {
                    if (this.Any())
                    {
                        options.SinceId = this.First().Id;
                    }
                }
                else
                {
                    if (_maxId > 0)
                    {
                        options.MaxId = _maxId;
                    }
                }

                statuses = await _client.GetNotifications(options);
            }
            catch (Exception e)
            {
                // TODO: Show error
                Console.WriteLine(e);
                statuses = new List<Notification>();
            }
            IsLoading = false;
            return statuses;
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            var statuses = await RefreshTimeline(false);
            if (statuses.Any())
            {
                this.AddRange(statuses);
                _maxId = statuses.Last().Id;
            }
            else
            {
                HasMoreItems = false;
            }
            return new LoadMoreItemsResult { Count = count };
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }

        private int _sinceId;

        private int _maxId;

        private MastodonClient _client;

        public bool HasMoreItems { get; set; }
    }
}
