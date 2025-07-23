using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cornometer : ContentPage
    {
        System.Timers.Timer timer;
        TimerTime time = new TimerTime();
        public ObservableCollection<TimerTime> items = new ObservableCollection<TimerTime>();

        public Cornometer()
        {
            InitializeComponent();
            TxtDay.Text = "00";
            TxtHour.Text = "00";
            TxtMin.Text = "00";
            TxtSec.Text = "00";
            LapList.ItemsSource = items;
        }

        private void RefreshLapList()
        {
            LapList.ItemsSource = null;
            LapList.ItemsSource = new ObservableCollection<TimerTime>(items.Reverse());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 100;
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Enabled = true;
                time.SetTime(new TimerTime(Convert.ToInt32(TxtDay.Text), Convert.ToInt32(TxtHour.Text),
                    Convert.ToInt32(TxtMin.Text), Convert.ToDouble(TxtSec.Text)));
                Update();
                BtnStart.Text = "pause";
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
                            Convert.ToInt32(TxtMin.Text), Convert.ToDouble(TxtSec.Text)));
                    }
                }
                Update();
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TxtSec.Text = (Convert.ToDouble(TxtSec.Text) + 0.1).ToString();
                Update();
            });
        }

        private void Update()
        {
            if (timer != null)
            {
                int day = Convert.ToInt32(TxtDay.Text);
                int hour = Convert.ToInt32(TxtHour.Text);
                int min = Convert.ToInt32(TxtMin.Text);
                double sec = Convert.ToDouble(TxtSec.Text);
                if (sec > 59.9)
                {
                    sec = 0;
                    min++;
                }
                if (min > 59)
                {
                    min = 0;
                    hour++;
                }
                if (hour > 23)
                {
                    hour = 0;
                    day++;
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
                Convert.ToInt32(TxtMin.Text), Convert.ToDouble(TxtSec.Text)));
            if (items.Count > 0)
            {
                ListHeader.IsVisible = true;
            }
            RefreshLapList();
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
        public string Beautify(double value)
        {
            return value < 10 ? "0" + value.ToString() : value.ToString();
        }
    }
}