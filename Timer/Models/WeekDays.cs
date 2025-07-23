using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timer.Models
{
    [Table("WeekDays")]
    public class WeekDays
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
        public bool sunday { get; set; }

        public WeekDays() { }

        //public WeekDays(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        //{
        //    id = 0;
        //    this.monday = monday;
        //    this.tuesday = tuesday;
        //    this.wednesday = wednesday;
        //    this.thursday = thursday;
        //    this.friday = friday;
        //    this.saturday = saturday;
        //    this.sunday = sunday;
        //}
        //public WeekDays(WeekDays weekDays)
        //{
        //    id = 0;
        //    monday = weekDays.monday;
        //    tuesday = weekDays.tuesday;
        //    wednesday = weekDays.wednesday;
        //    thursday = weekDays.thursday;
        //    friday = weekDays.friday;
        //    saturday = weekDays.saturday;
        //    sunday = weekDays.sunday;
        //}
        //public WeekDays(int id, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        //{
        //    this.id = id;
        //    this.monday = monday;
        //    this.tuesday = tuesday;
        //    this.wednesday = wednesday;
        //    this.thursday = thursday;
        //    this.friday = friday;
        //    this.saturday = saturday;
        //    this.sunday = sunday;
        //}
        //public WeekDays(int id, WeekDays weekDays)
        //{
        //    this.id = id;
        //    monday = weekDays.monday;
        //    tuesday = weekDays.tuesday;
        //    wednesday = weekDays.wednesday;
        //    thursday = weekDays.thursday;
        //    friday = weekDays.friday;
        //    saturday = weekDays.saturday;
        //    sunday = weekDays.sunday;
        //}
    }
}
