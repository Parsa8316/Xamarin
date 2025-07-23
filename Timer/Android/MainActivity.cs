using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Android.Content;
using Android.Media;
using AndroidX.Core.App;
using Xamarin.Essentials;
using Timer.ViewModels;
using Timer.Models;

namespace Timer.Droid
{
    [Activity(Label = "Timer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //NotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
            //{
            //    Sound = "1623497558_S8nC4", // بدون .mp3
            //    Importance = NotificationImportance.High,
            //    EnableVibration = true,
            //    ShowBadge = true,
            //    Name = "TimerChannel",
            //    Description = "Channel for Timer Alerts",
            //    Id = "timer_channel"
            //});

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var soundUri = Android.Net.Uri.Parse($"android.resource://{PackageName}/raw/alarm");

                var audioAttr = new AudioAttributes.Builder()
                    .SetContentType(AudioContentType.Sonification)
                    .SetUsage(AudioUsageKind.Notification)
                    .Build();

                var channel = new NotificationChannel("timer_channel", "Timer Alerts", NotificationImportance.High)
                {
                    Description = "Alarm for timer"
                };
                channel.SetSound(soundUri, audioAttr);

                var manager = (NotificationManager)GetSystemService(NotificationService);
                manager.CreateNotificationChannel(channel);
                //Vibration.Vibrate(TimeSpan.FromSeconds(5));
                //SetAlarm(0);
            }

            LoadApplication(new App());
        }


        public void ChangeTheme()
        {
            switch (Setting.Theme)
            {
                case 1:
                    Window.SetStatusBarColor(Android.Graphics.Color.White);
                    break;
                case -1:
                    Window.SetStatusBarColor(Android.Graphics.Color.Black);
                    break;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void SetAlarm(int seconds)
        {
            var intent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            intent.PutExtra("message", "your time is up");

            var pending = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var alarmTime = Java.Lang.JavaSystem.CurrentTimeMillis() + seconds * 1000;

            var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            alarmManager.SetExact(AlarmType.RtcWakeup, alarmTime, pending);
        }

        public class Environment1 : IEnvironment
        {
            public async void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
            {
                if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                    return;

                var activity = Platform.CurrentActivity;
                var window = activity.Window;

                //this may not be necessary(but may be fore older than M)
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                window.SetStatusBarColor(color.ToPlatformColor());


                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                    window.DecorView.SystemUiVisibility = darkStatusBarTint ? flag : 0;
                }

            }
        }



        [BroadcastReceiver(Enabled = true, Exported = true)]
        public class AlarmReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                var soundUri = Android.Net.Uri.Parse($"android.resource://{context.PackageName}/raw/alarm");

                var builder = new NotificationCompat.Builder(context, "timer_channel")
                    .SetContentTitle("your time is up")
                    .SetContentText("time to wake up")
                    .SetSmallIcon(Resource.Drawable.icon_feed)
                    .SetAutoCancel(true)
                    .SetSound(soundUri)
                    .SetPriority((int)NotificationPriority.High)
                    .SetDefaults((int)NotificationDefaults.All);

                var notificationManager = NotificationManagerCompat.From(context);
                notificationManager.Notify(1001, builder.Build());
            }
        }


    }
}