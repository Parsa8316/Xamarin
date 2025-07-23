using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Timer.Models;
using Xamarin.Essentials;

namespace Timer.Services
{
    public class PinService
    {
        static SQLiteAsyncConnection db;
        async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<PinnedItems>();
        }
        public async Task Add(int pinned)
        {
            await Init();
            List<PinnedItems> _ = await db.Table<PinnedItems>().Where(i => i.pinnedId == pinned).ToListAsync();
            if (_.Count == 0)
            {
                await db.InsertAsync(new PinnedItems(pinned));
            }
        }
        public async Task Remove(int id)
        {
            await Init();
            //if ((await Get(id)) != null)
            //{
            await db.DeleteAsync<PinnedItems>(id);
            //}
        }
        public async Task<List<PinnedItems>> GetAll()
        {
            await Init();
            return await db.Table<PinnedItems>().ToListAsync();
        }
        public async Task<PinnedItems> Get(int pinned)
        {
            await Init();
            return await db.Table<PinnedItems>().FirstOrDefaultAsync(i => i.pinnedId == pinned);
        }
    }
}
