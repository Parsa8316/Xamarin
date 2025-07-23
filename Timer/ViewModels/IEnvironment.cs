using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Timer.ViewModels
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
