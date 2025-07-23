using Plugin.LocalNotification;
using Remotion.Linq.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Timer.Models;
using Timer.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClockTimer : ContentPage
    {
        public ObservableCollection<Models.ClockTimer> items { get; set; }
        public ObservableCollection<Models.ClockTimer> pinned { get; set; }

        System.Timers.Timer timer;
        System.Timers.Timer timerOneWeek;
        TimerTime time = new TimerTime();


        public ClockTimer()
        {
            InitializeComponent();
        }

        private async void RefreshList()
        {
            ClockTimerService db = new ClockTimerService();
            PinService pinService = new PinService();
            MainList.ItemsSource = null;
            items = new ObservableCollection<Models.ClockTimer>();
            pinned = new ObservableCollection<Models.ClockTimer>();
            List<PinnedItems> pinItems = await pinService.GetAll();
            foreach (var pin in pinItems)
            {
                var cloclTimerPinned = await db.Get(pin.pinnedId);
                pinned.Add(new Models.ClockTimer(cloclTimerPinned));
            }
            foreach (var item in pinned.Reverse())
            {
                item.IsPinned = true;
                items.Add(item);
            }
            foreach (var item in await db.GetAll())
            {
                if (!pinned.Any(i => i.id == item.id))
                {
                    item.IsPinned = false;
                    items.Add(item);
                }
            }
            MainList.ItemsSource = new ObservableCollection<Models.ClockTimer>(items);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //ClockTimerService db = new ClockTimerService();
            //items = new ObservableCollection<Models.ClockTimer>();
            //var all = await db.GetAll();
            //foreach (Models.ClockTimer clockTimer in all)
            //{
            //    items.Add(clockTimer);
            //}
            RefreshList();

            if (timer == null)
            {
                try
                {
                    timer = new System.Timers.Timer();
                    timer.Interval = 1000;
                    timer.Elapsed += OnTimedEvent;
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    DateTime now = DateTime.Now;
                    time.SetTime(new TimerTime(now.Day, now.Hour, now.Minute, now.Second));
                }
                catch { }
            }
            if (timerOneWeek == null)
            {
                try
                {
                    timer = new System.Timers.Timer();
                    timer.Interval = 604800000;
                    timer.Elapsed += OnTimedEventWeek;
                    timer.AutoReset = true;
                    timer.Enabled = true;
                }
                catch { }
            }
        }

        string[] days = new string[7] { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ClockTimerService db = new ClockTimerService();
            Device.BeginInvokeOnMainThread(async () =>
            {
                PersianDateTime now = PersianDateTime.Now;
                time.SetTime(new TimerTime(now.Day, now.Hour, now.Minute, now.Second));
                if (now.Second == 0)
                {
                    timer.Interval = 60000;
                }
                foreach (var item in items)
                {
                    if (item.IsOn)
                    {
                        WeekDays d = await db.GetWeekDays(item.WeekDays);
                        bool[] daysB = new bool[7] { d.monday, d.tuesday, d.wednesday, d.thursday, d.friday, d.saturday, d.sunday };
                        int dayOfWeek = days.IndexOf(DateTime.Now.DayOfWeek.ToString().ToLower());
                        if (daysB[dayOfWeek])
                        {
                            TimerTime clock = new TimerTime(0, item.Time.Hours, item.Time.Minutes, 0);
                            if (clock.hour == now.Hour && clock.minute == now.Minute)
                            {
                                DisplayAlert("Done", "time is " + item.Time, "ok");
                                Vibration.Vibrate(TimeSpan.FromSeconds(2));
                                var notification = new Plugin.LocalNotification.NotificationRequest
                                {
                                    NotificationId = 1001,
                                    Title = "Done",
                                    Description = "time is" + item.Time,
                                    Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                                    {
                                        ChannelId = "timer_channel",
                                    }
                                };
                                NotificationCenter.Current.Show(notification);
                            }
                        }
                    }
                }
            });
        }
        private async void OnTimedEventWeek(Object source, ElapsedEventArgs e)
        {
            ClockTimerService db = new ClockTimerService();
            Device.BeginInvokeOnMainThread(async () =>
            {
                foreach (var item in items)
                {
                    if (item.IsOn && !item.Repeat)
                    {
                        item.IsOn = false;
                        await db.Update(item);
                    }
                }
            });
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            string route = $"{nameof(AddClockTimer)}?clockTimerIdStr={-1}";
            await Shell.Current.GoToAsync(route);
        }

        private void RdIsOn_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ClockTimerService db = new ClockTimerService();
            CheckBox checkbox = sender as CheckBox;

            Models.ClockTimer item = checkbox?.BindingContext as Models.ClockTimer;
            if (item != null)
            {
                db.Update(item);
            }
        }

        private async void MainList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Models.ClockTimer clockTimer = ((Xamarin.Forms.ListView)sender).SelectedItem as Models.ClockTimer;
            int id = 0;
            if (clockTimer != null) id = clockTimer.id;

            string route = $"{nameof(AddClockTimer)}?clockTimerIdStr={id}";
            await Shell.Current.GoToAsync(route);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            Models.ClockTimer clockTimer = ((MenuItem)sender).BindingContext as Models.ClockTimer;
            ClockTimerService db = new ClockTimerService();
            PinService pinService = new PinService();
            PinnedItems pin = await pinService.Get(clockTimer.id);

            await db.Remove(clockTimer.id);
            if (pin != null)
            {
                await pinService.Remove(pin.id);
            }
            //items.Remove(clockTimer);
            RefreshList();
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Models.ClockTimer clockTimer = menuItem.BindingContext as Models.ClockTimer;
            PinService pinService = new PinService();
            var all = await pinService.GetAll();
            if (all.Any(i => i.pinnedId == clockTimer.id))
            {
                PinnedItems pinnedItems = await pinService.Get(clockTimer.id);
                await pinService.Remove(pinnedItems.id);
                menuItem.Text = "Pin";
            }
            else
            {
                await pinService.Add(clockTimer.id);
                menuItem.Text = "Unpin";
            }
            RefreshList();
        }
    }
}