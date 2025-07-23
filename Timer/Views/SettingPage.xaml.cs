using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer.Models;
using Timer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public int theme { get; set; }

        public SettingPage()
        {
            InitializeComponent();

            switch (Setting.Theme)
            {
                case 0:
                    break;
                case 1:
                    RdL.IsChecked = true;
                    break;
                case -1:
                    RdD.IsChecked = true;
                    break;

            }
        }

        private void RdL_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value) return;
            string val = (sender as RadioButton)?.Value as string;
            if (string.IsNullOrEmpty(val)) return;
            switch (val)
            {
                case "Light":
                    Setting.Theme = 1;
                    break;
                case "Dark":
                    Setting.Theme = -1;
                    break;
            }
            Theme.SetTheme();
        }
    }
}