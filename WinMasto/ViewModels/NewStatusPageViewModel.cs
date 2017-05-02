using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mastonet;
using Mastonet.Entities;
using WinMasto.Models;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class NewStatusPageViewModel : WinMastoViewModel
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;

            var statusParameters = parameter as NewStatusParameter;
            if (statusParameters != null)
            {
                if (statusParameters.IsMention)
                {
                    Status += $"@{statusParameters.Status.Account.AccountName}";
                }

                if (statusParameters.IsReply)
                {
                    ReplyStatus = statusParameters.Status;
                    Status += $"@{statusParameters.Status.Account.AccountName}";
                }
            }

            await LoginUser();
            PhotoList = new ObservableCollection<PhotoFileInfo>();
            IsLoading = false;
        }

        private ObservableCollection<PhotoFileInfo> _photoList;

        public ObservableCollection<PhotoFileInfo> PhotoList
        {
            get { return _photoList; }
            set
            {
                Set(ref _photoList, value);
            }
        }

        private bool _sensitive = default(bool);

        public bool Sensitive
        {
            get { return _sensitive; }
            set
            {
                Set(ref _sensitive, value);
            }
        }

        private Status _replyStatus;

        public Status ReplyStatus
        {
            get { return _replyStatus; }
            set
            {
                Set(ref _replyStatus, value);
            }
        }

        private string _status = "";

        public string Status
        {
            get { return _status; }
            set
            {
                Set(ref _status, value);
            }
        }

        private string _spoilerText = "";

        public string SpoilerText
        {
            get { return _spoilerText; }
            set
            {
                Set(ref _spoilerText, value);
            }
        }

        private int _statusCount = 500;

        public int StatusCount
        {
            get { return _statusCount; }
            set
            {
                Set(ref _statusCount, value);
            }
        }

        public void StatusTextBox_OnChanged(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            StatusCount = 500 - sender.Text.Length;
        }

        public async Task SendStatus()
        {
            // TODO: This is for testing the postStatus function. Make this more generic.
            IsLoading = true;
            if (string.IsNullOrEmpty(Status) || Status.Length > 500) return;
            IEnumerable<int> mediaIds = null;
            if (PhotoList.Any())
            {
                mediaIds = PhotoList.Select(node => node.Attachment.Id);
            }
            int? replyId = null;
            if (ReplyStatus != null) replyId = ReplyStatus.Id;
            try
            {
                var result = await Client.PostStatus(Status, Visibility.Public, replyId, mediaIds, Sensitive, SpoilerText.Any() ? SpoilerText : null);
                await NavigationService.NavigateAsync(typeof(MainPage));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            IsLoading = false;
        }
    }
}
