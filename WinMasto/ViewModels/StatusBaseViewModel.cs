using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet;
using Mastonet.Entities;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class StatusBaseViewModel : WinMastoViewModel
    {
        internal TimelineStreaming _timelineStreaming;

        public ObservableCollection<Status> Statuses { get; set; }

        public async Task ReplyOption(Status status)
        {

        }

        public async Task MentionOption(Status status)
        {

        }

        public async Task BlockOption(Status status)
        {

        }

        public async Task MuteOption(Status status)
        {

        }

        public async Task ReportOption(Status status)
        {

        }

        public async Task ReShareOption(Status status)
        {
            // TODO: This "works", but it could be more simple. The API layer needs to be tweeked.
            // It would make more sense to replace the status object in the list with the one it gets from the API
            // But it's not updating, because OnPropertyChanged is not in Status...
            // Reblog returns "reblogged" status, not the original status updated.
            Status newStatus = status;
            if (status.Reblogged == null)
            {
                await Client.Reblog(status.Id);
                newStatus.Reblogged = true;
                newStatus.ReblogCount = newStatus.ReblogCount + 1;
            }
            else
            {
                var reblogged = !status.Reblogged.Value;
                if (reblogged)
                {
                    await Client.Reblog(status.Id);
                    newStatus.Reblogged = true;
                    newStatus.ReblogCount = newStatus.ReblogCount + 1;
                }
                else
                {
                    await Client.Unreblog(status.Id);
                    newStatus.Reblogged = false;
                    newStatus.ReblogCount = newStatus.ReblogCount - 1;
                }
            }
            var index = Statuses.IndexOf(status);
            Statuses[index] = newStatus;
        }

        public async Task FavoriteOption(Status status)
        {
            // TODO: This "works", but it could be more simple. The API layer needs to be tweeked.
            // It would make more sense to replace the status object in the list with the one it gets from the API
            // But it's not updating, because OnPropertyChanged is not in Status...
            Status newStatus;
            if (status.Favourited == null)
            {
                newStatus = await Client.Favourite(status.Id);
            }
            else
            {
                var favorite = !status.Favourited.Value;
                if (favorite)
                {
                    newStatus = await Client.Favourite(status.Id);
                }
                else
                {

                    // API Bug: Unfavorite returns a status that still says it's favorited, even though it's not.
                    // Not sure if it's mastodon, or the instance I'm on. So for now, we'll force it to say it's
                    // not there.
                    newStatus = await Client.Unfavourite(status.Id);
                    newStatus.Favourited = false;
                    newStatus.FavouritesCount = newStatus.FavouritesCount - 1;
                }
            }
            var index = Statuses.IndexOf(status);
            Statuses[index] = newStatus;
        }

        public async Task NavigateToLoginView()
        {
            await NavigationService.NavigateAsync(typeof(LoginPage));
        }
    }
}
