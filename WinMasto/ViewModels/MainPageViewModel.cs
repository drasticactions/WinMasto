﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mastonet;
using Mastonet.Entities;
using Template10.Services.NavigationService;
using Template10.Utils;
using WinMasto.Views;

namespace WinMasto.ViewModels
{
    public class MainPageViewModel : WinMastoViewModel
    {
        private TimelineStreaming _timelineStreaming;

        public ObservableCollection<Status> Statuses { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await LoginUser();
            if (IsLoggedIn)
            {
                Statuses = new ObservableCollection<Status>();
                //_timelineStreaming = Client.GetUserStreaming();
                //_timelineStreaming.OnUpdate += TimelineStreamingOnUpdate;
                //_timelineStreaming.OnDelete += TimelineStreamingOnDelete;
                //_timelineStreaming.OnNotification += TimelineStreamingOnNotification;
                //_timelineStreaming.Start();
                var statuses = await Client.GetHomeTimeline();
                Statuses.AddRange(statuses);
            }
            IsLoading = false;
        }

        private void TimelineStreamingOnNotification(object sender, StreamNotificationEventArgs streamNotificationEventArgs)
        {
            
        }

        private void TimelineStreamingOnDelete(object sender, StreamDeleteEventArgs streamDeleteEventArgs)
        {
            
        }

        private void TimelineStreamingOnUpdate(object sender, StreamUpdateEventArgs streamUpdateEventArgs)
        {
           
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            if (IsLoggedIn)
            {
                //_timelineStreaming.OnUpdate -= TimelineStreamingOnUpdate;
                //_timelineStreaming.OnDelete -= TimelineStreamingOnDelete;
                //_timelineStreaming.OnNotification -= TimelineStreamingOnNotification;
                //_timelineStreaming.Stop();
            }

            return base.OnNavigatingFromAsync(args);
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

        public async Task NavigateToLoginView()
        {
            await NavigationService.NavigateAsync(typeof(LoginPage));
        }
    }
}
