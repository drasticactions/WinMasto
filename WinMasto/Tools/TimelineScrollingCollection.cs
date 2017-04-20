using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Mastonet;
using Mastonet.Entities;
using System.ComponentModel;
using Template10.Utils;

namespace WinMasto.Tools
{
    public class TimelineScrollingCollection : ObservableCollection<Status>, ISupportIncrementalLoading
    {
        public TimelineScrollingCollection(MastodonClient client, string path, int accountId = 0)
        {
            HasMoreItems = true;
            IsLoading = false;
            _client = client;
            _path = path;
            _accountId = accountId;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            IEnumerable<Status> statuses;
            ArrayOptions options = new ArrayOptions();
            if (_maxId > 0)
            {
                options.MaxId = _maxId;
            }
            
            switch (_path)
            {
                case "home":
                    statuses = await _client.GetHomeTimeline(options);
                    break;
                case "public":
                    statuses = await _client.GetPublicTimeline(options);
                    break;
                case "local":
                    statuses = await _client.GetPublicTimeline(options, true);
                    break;
                case "account":
                    statuses = await _client.GetAccountStatuses(_accountId, options);
                    break;
                default:
                    statuses = await _client.GetHomeTimeline(options);
                    break;
            }
            if (statuses.Any())
            {
                this.AddRange(statuses);
                _maxId = statuses.Last().Id;
            }
            else
            {
                HasMoreItems = false;
            }
            IsLoading = false;
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

        private int _accountId;

        private MastodonClient _client;

        private string _path;

        public bool HasMoreItems { get; set; }
    }
}
