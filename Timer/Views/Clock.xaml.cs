using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clock : ContentPage
    {
        System.Timers.Timer timer;
        PersianDateTime time;

        public Clock()
        {
            InitializeComponent();

            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 100;
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Enabled = true;
                Update();
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Update();
            });
        }

        private void Update()
        {

            time = PersianDateTime.Now;
            TxtYear.Text = Beautify(time.Year);
            TxtMonth.Text = Beautify(time.Month);
            TxtDay.Text = Beautify(time.Day);
            TxtHour.Text = Beautify(time.Hour);
            TxtMin.Text = Beautify(time.Minute);
            TxtSec.Text = Beautify(time.Second) + "." + ((time.Millisecond - time.Millisecond%100)/100);

            //if (timer != null)
            //{
            //    //int day = Convert.ToInt32(TxtDay.Text);
            //    //int hour = Convert.ToInt32(TxtHour.Text);
            //    //int min = Convert.ToInt32(TxtMin.Text);
            //    //int sec = Convert.ToInt32(TxtSec.Text);
            //    //if (sec > 59)
            //    //{
            //    //    sec = 0;
            //    //    min++;
            //    //}
            //    //if (min > 59)
            //    //{
            //    //    min = 0;
            //    //    hour++;
            //    //}
            //    //if (hour > 23)
            //    //{
            //    //    hour = 0;
            //    //    day++;
            //    //}
            //}
        }
        public string Beautify(int value)
        {
            return value < 10 ? "0" + value.ToString() : value.ToString();
        }
    }
}