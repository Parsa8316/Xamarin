using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer.Models;
using Timer.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [QueryProperty(nameof(clockTimerIdStr), nameof(clockTimerIdStr))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddClockTimer : ContentPage
    {
        public string clockTimerIdStr { get; set; }
        public int clockTimerId = -1;

        public AddClockTimer()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int.TryParse(clockTimerIdStr, out clockTimerId);
            if (clockTimerId > 0)
            {
                BtnMain.Text = "Edit";
                ClockTimerService db = new ClockTimerService();
                var ct = await db.Get(clockTimerId);
                TxtHour.Text = ct.Time.Hours.ToString();
                TxtMinute.Text = ct.Time.Minutes.ToString();
                TxtRingtone.Text = ct.Ringtone;
                TxtExplanation.Text = ct.Explanation;
                TxtHowLongHour.Text = ct.HowLongToRepeat.Hours.ToString();
                TxtHowLongMinute.Text = ct.HowLongToRepeat.Minutes.ToString();
                WeekDays days = await db.GetWeekDays(ct.WeekDays);
                RdMod.IsChecked = days.monday;
                RdTue.IsChecked = days.tuesday;
                RdWed.IsChecked = days.wednesday;
                RdThu.IsChecked = days.thursday;
                RdFri.IsChecked = days.friday;
                RdSat.IsChecked = days.saturday;
                RdSun.IsChecked = days.sunday;
                RdIsOn.IsChecked = ct.IsOn;
                RdRepeat.IsChecked = ct.Repeat;
            }
            else
            {
                BtnMain.Text = "Add";
            }
        }

        private async void BtnMain_Clicked(object sender, EventArgs e)
        {
            ClockTimerService db = new ClockTimerService();
            WeekDays weekDays = new WeekDays
            {
                id = 0,
                monday = RdMod.IsChecked,
                tuesday = RdTue.IsChecked,
                wednesday = RdWed.IsChecked,
                thursday = RdThu.IsChecked,
                friday = RdFri.IsChecked,
                saturday = RdSat.IsChecked,
                sunday = RdSun.IsChecked
            };
            try
            {
                if (clockTimerId > 0)
                {
                    await db.Update(clockTimerId, RdRepeat.IsChecked, weekDays, TxtExplanation.Text,
                        new TimeSpan(Convert.ToInt32(TxtHour.Text), Convert.ToInt32(TxtMinute.Text), 0), TxtRingtone.Text,
                        RdIsOn.IsChecked, new TimeSpan(Convert.ToInt32(TxtHowLongHour.Text), Convert.ToInt32(TxtHowLongMinute.Text), 0));
                }
                else
                {
                    await db.Add(RdRepeat.IsChecked, weekDays, TxtExplanation.Text,
                        new TimeSpan(Convert.ToInt32(TxtHour.Text), Convert.ToInt32(TxtMinute.Text), 0), TxtRingtone.Text,
                        RdIsOn.IsChecked, new TimeSpan(Convert.ToInt32(TxtHowLongHour.Text), Convert.ToInt32(TxtHowLongMinute.Text), 0));
                }
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }
    }
}