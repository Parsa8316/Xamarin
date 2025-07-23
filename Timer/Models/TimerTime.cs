using System;
using System.Collections.Generic;
using System.Text;

namespace Timer.Models
{
    public class TimerTime
    {
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int deciSecond { get; set; }

        public double secondPlusDeci {  get; set; }

        public TimerTime(int day, int hour, int minute, int second)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            secondPlusDeci = second;
            deciSecond = 0;
        }
        public TimerTime(int day, int hour, int minute, double second)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = (int)(second - second%1);
            deciSecond = (int)Math.Floor((second%1)*10);
            secondPlusDeci = second;
        }
        public TimerTime(int day, int hour, int minute, int second,int deciSecond)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.deciSecond = deciSecond;
            secondPlusDeci = second + deciSecond;
        }
        public TimerTime() { }
        public TimerTime GetTime()
        {
            return new TimerTime(day, hour, minute, second, deciSecond);
        }
        public void SetTime(TimerTime time)
        {
            day = time.day;
            hour = time.hour;
            minute = time.minute;
            second = time.second;
            deciSecond = time.deciSecond;
            secondPlusDeci = time.secondPlusDeci;
        }
        public string GetDay() { return day.ToString(); }
        public string GetHour() { return hour.ToString(); }
        public string GetMinute() { return minute.ToString(); }
        public string GetSecond() { return second.ToString(); }

        public string ToString()
        {
            return $"{day}:{hour}:{day}:{second}";
        }
    }
}
