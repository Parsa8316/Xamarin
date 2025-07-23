using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;
using Timer.Models;
using Xamarin.Forms;

namespace Timer.ViewModels
{
    public static class Theme
    {
        public static void SetTheme()
        {
            switch (Setting.Theme)
            {
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case 1:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case -1:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;

            }

            var nav = App.Current.MainPage as Xamarin.Forms.NavigationPage;
            var e = DependencyService.Get<IEnvironment>();
            if (App.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColor(Color.Black, false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.Black;
                    nav.BarTextColor = Color.White;
                }
            }
            else
            {
                e?.SetStatusBarColor(Color.White, true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.White;
                    nav.BarTextColor = Color.Black;
                }
            }
        }
    }
}
