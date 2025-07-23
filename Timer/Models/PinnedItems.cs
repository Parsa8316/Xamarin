using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;

namespace Timer.Models
{
    public class PinnedItems
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int pinnedId { get; set; }

        public PinnedItems() { }
        public PinnedItems(int id, int pinnedId, bool IsPinned)
        {
            this.id = id;
            this.pinnedId = pinnedId;
        }
        public PinnedItems(int pinnedId)
        {
            this.pinnedId = pinnedId;
        }
    }
}
