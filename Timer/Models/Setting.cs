using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Timer.Models
{
    public static class Setting
    {
        const int theme = 1;
        public static int Theme
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }
    }
}
