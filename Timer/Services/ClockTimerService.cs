using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer.Models;
using Xamarin.Essentials;

namespace Timer.Services
{
    public class ClockTimerService
    {
        static SQLiteAsyncConnection db;
        async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<WeekDays>();
            await db.CreateTableAsync<ClockTimer>();
        }
        public async Task<ClockTimer> Add(bool repeat, WeekDays days, string explanation, TimeSpan time, string ringtone, bool isOn, TimeSpan howLongToRepeat)
        {
            await Init();
            WeekDays x = await db.Table<WeekDays>().FirstOrDefaultAsync(i => i.monday == days.monday && i.saturday == days.saturday &&
                    i.sunday == days.sunday && i.tuesday == days.tuesday && i.wednesday == days.wednesday && i.thursday == days.thursday
                    && i.friday == days.friday);
            if (x == null || x.id <= 0)
            {
                await db.InsertAsync(days);
                x = await db.Table<WeekDays>().FirstOrDefaultAsync(i => i.monday == days.monday && i.saturday == days.saturday &&
                    i.sunday == days.sunday && i.tuesday == days.tuesday && i.wednesday == days.wednesday && i.thursday == days.thursday
                    && i.friday == days.friday);
            }
            ClockTimer clockTimer = new ClockTimer(repeat, x.id, explanation, time, ringtone, isOn, howLongToRepeat);
            int Id = await db.InsertAsync(clockTimer);
            return clockTimer;
        }
        public async Task Remove(int id)
        {
            await Init();
            //if (Get(id). != null)
            //{
            await db.DeleteAsync<ClockTimer>(id);
            //}
        }
        public async Task Update(int id, bool repeat, WeekDays days, string explanation, TimeSpan time, string ringtone, bool isOn, TimeSpan howLongToRepeat)
        {
            await Init();
            ClockTimer ct = await db.GetAsync<ClockTimer>(id);
            ct.Repeat = repeat;
            WeekDays x = await db.Table<WeekDays>().FirstOrDefaultAsync(i => i.monday == days.monday && i.saturday == days.saturday &&
                    i.sunday == days.sunday && i.tuesday == days.tuesday && i.wednesday == days.wednesday && i.thursday == days.thursday
                    && i.friday == days.friday);
            if (x == null || x.id <= 0)
            {
                await db.InsertAsync(days);
                x = await db.Table<WeekDays>().FirstOrDefaultAsync(i => i.monday == days.monday && i.saturday == days.saturday &&
                    i.sunday == days.sunday && i.tuesday == days.tuesday && i.wednesday == days.wednesday && i.thursday == days.thursday
                    && i.friday == days.friday);
            }
            ct.WeekDays = x.id;
            ct.Explanation = explanation;
            ct.Time = time;
            ct.Ringtone = ringtone;
            ct.IsOn = isOn;
            ct.HowLongToRepeat = howLongToRepeat;
            await db.UpdateAsync(ct);
        }
        public async Task Update(ClockTimer clockTimer)
        {
            await Init();
            ClockTimer ct = await db.GetAsync<ClockTimer>(clockTimer.id);
            ct.Repeat = clockTimer.Repeat;
            ct.WeekDays = clockTimer.WeekDays;
            ct.Explanation = clockTimer.Explanation;
            ct.Time = clockTimer.Time;
            ct.Ringtone = clockTimer.Ringtone;
            ct.IsOn = clockTimer.IsOn;
            ct.HowLongToRepeat = clockTimer.HowLongToRepeat;
            await db.UpdateAsync(ct);
        }
        public async Task<List<ClockTimer>> GetAll()
        {
            await Init();
            var x = await db.Table<ClockTimer>().ToListAsync();
            return x.OrderBy(i => i.Time).ToList();
        }
        public async Task<ClockTimer> Get(int id)
        {
            await Init();
            return await db.Table<ClockTimer>().FirstOrDefaultAsync(i => i.id == id);
        }
        public async Task<WeekDays> GetWeekDays(int id)
        {
            await Init();
            return await db.Table<WeekDays>().FirstOrDefaultAsync(i => i.id == id);
        }
    }
}
