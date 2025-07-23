using Android.Content;
using Android.OS;
using Android.Webkit;
using MediaBrowser.Model.Notifications;
using MediaManager;
using Microsoft.VisualStudio.Services.ClientNotification;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timer : ContentPage
    {
        System.Timers.Timer timer;
        TimerTime time = new TimerTime();
        public ObservableCollection<TimerTime> items = new ObservableCollection<TimerTime>();

        public Timer()
        {
            InitializeComponent();
            //time.SetTime(new TimerTime(Convert.ToInt32(TxtDay.Text), Convert.ToInt32(TxtHour.Text),
            //    Convert.ToInt32(TxtMin.Text), Convert.ToInt32(TxtSec.Text)));

            //var items1 = items.Reverse();
            LapList.ItemsSource = items;
            //LapList.Reverse = true;

        }
        private void RefreshLapList()
        {
            LapList.ItemsSource = null;
            LapList.ItemsSource = new ObservableCollection<TimerTime>(items.Reverse());
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            if (timer == null)
            {
                try
                {
                    timer = new System.Timers.Timer();
                    timer.Interval = 1000;
                    timer.Elapsed += OnTimedEvent;
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    time.SetTime(new TimerTime(Convert.ToInt32(TxtDay.Text), Convert.ToInt32(TxtHour.Text),
                        Convert.ToInt32(TxtMin.Text), Convert.ToInt32(TxtSec.Text)));
                    Update();
                    BtnStart.Text = "pause";
                }
                catch { }
            }
            else
            {
                string text = BtnStart.Text;
                if (text == "pause")
                {
                    timer.Stop();
                    BtnStart.Text = "resume";
                }
                else
                {
                    timer.Start();
                    BtnStart.Text = "pause";
                    if (BtnStart.Text == "start")
                    {
                        time.SetTime(new TimerTime(Convert.ToInt32(TxtDay.Text), Convert.ToInt32(TxtHour.Text),
                            Convert.ToInt32(TxtMin.Text), Convert.ToInt32(TxtSec.Text)));
                    }
                }
                Update();
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TxtSec.Text = (Convert.ToInt32(TxtSec.Text) - 1).ToString();
                Update();
            });
        }

        private async Task Update()
        {
            if (timer != null)
            {
                int day = Convert.ToInt32(TxtDay.Text);
                int hour = Convert.ToInt32(TxtHour.Text);
                int min = Convert.ToInt32(TxtMin.Text);
                int sec = Convert.ToInt32(TxtSec.Text);
                if (sec < 0)
                {
                    sec = 59;
                    min--;
                }
                if (min < 0)
                {
                    min = 59;
                    hour--;
                }
                if (hour < 0)
                {
                    hour = 23;
                    day--;
                }
                if (day < 0)
                {
                    day = 0;
                    hour = 0;
                    min = 0;
                    sec = 0;
                    timer.Stop();
                    DisplayAlert("Done", "your time is up!", "ok");
                    Vibration.Vibrate(TimeSpan.FromSeconds(2));

                    //var player = CrossMediaManager.Current;
                    //player.Play("views:///alarm.mp3");


                    var notification = new Plugin.LocalNotification.NotificationRequest
                    {
                        NotificationId = 1001,
                        Title = "Done",
                        Description = "your time is up",
                        Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                        {
                            ChannelId = "timer_channel",
                        }
                    };
                    NotificationCenter.Current.Show(notification);
                    //ScheduleAlarm(10);

                    BtnStart.Text = "start";
                }
                TxtSec.Text = Beautify(sec);
                TxtMin.Text = Beautify(min);
                TxtHour.Text = Beautify(hour);
                TxtDay.Text = Beautify(day);
            }
        }

        private void BtnLop_Clicked(object sender, EventArgs e)
        {
            items.Add(new TimerTime(Convert.ToInt32(TxtDay.Text), Convert.ToInt32(TxtHour.Text),
                Convert.ToInt32(TxtMin.Text), Convert.ToInt32(TxtSec.Text)));
            if (items.Count > 0)
            {
                ListHeader.IsVisible = true;
            }
            RefreshLapList();
        }

        //public ObservableCollection<TimerT> ReverseItems()
        //{
        //    var items1 = new ObservableCollection<TimerTime>();
        //    for (int i = items.Count - 1; i >= 0; i--)
        //    {
        //        items1.Add(items[i]);
        //    }
        //    items = items1;
        //}

        private void BtnReset_Clicked(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                BtnStart.Text = "start";
                TxtDay.Text = Beautify(time.GetDay());
                TxtHour.Text = Beautify(time.GetHour());
                TxtMin.Text = Beautify(time.GetMinute());
                TxtSec.Text = Beautify(time.GetSecond());
                items = new ObservableCollection<TimerTime>();
                RefreshLapList();
            }
        }

        private void BtnAgain_Clicked(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                time.SetTime(new TimerTime(0, 0, 0, 0));
                BtnStart.Text = "start";
                items = new ObservableCollection<TimerTime>();
                ListHeader.IsVisible = false;
                TxtDay.Text = "00";
                TxtHour.Text = "00";
                TxtMin.Text = "00";
                TxtSec.Text = "00";
                RefreshLapList();
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as TimerTime;
            if (item != null)
            {
                items.Remove(item);
            }
            RefreshLapList();
            if (items.Count == 0)
            {
                ListHeader.IsVisible = false;
            }
        }

        public string Beautify(int value)
        {
            return value < 10 ? "0" + value.ToString() : value.ToString();
        }
        public string Beautify(string value)
        {
            return Beautify(Convert.ToInt32(value));
        }
    }
}