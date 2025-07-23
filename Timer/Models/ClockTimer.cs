using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timer.Models
{
    [Table("ClockTimer")]
    public class ClockTimer
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public bool Repeat { get; set; }
        public int WeekDays { get; set; }
        public string Explanation { get; set; }
        public TimeSpan Time { get; set; }
        public string Ringtone { get; set; }
        public bool IsOn { get; set; }
        public TimeSpan HowLongToRepeat { get; set; }
        public bool IsPinned { get; set; }

        public ClockTimer() { }
        public ClockTimer(int id, bool repeat, int days, string explanation, TimeSpan time, string ringtone, bool isOn, TimeSpan howLongToRepeat)
        {
            this.id = id;
            Repeat = repeat;
            WeekDays = days;
            Explanation = explanation;
            Time = time;
            Ringtone = ringtone;
            IsOn = isOn;
            HowLongToRepeat = howLongToRepeat;
        }
        public ClockTimer(bool repeat, int days, string explanation, TimeSpan time, string ringtone, bool isOn, TimeSpan howLongToRepeat)
        {
            Repeat = repeat;
            WeekDays = days;
            Explanation = explanation;
            Time = time;
            Ringtone = ringtone;
            IsOn = isOn;
            HowLongToRepeat = howLongToRepeat;
        }
        public ClockTimer(ClockTimer other)
        {
            this.id = other.id;
            Repeat = other.Repeat;
            WeekDays = other.WeekDays;
            Explanation = other.Explanation;
            Time = other.Time;
            Ringtone = other.Ringtone;
            IsOn = other.IsOn;
            HowLongToRepeat = other.HowLongToRepeat;
        }
    }
}
