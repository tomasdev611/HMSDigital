using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.DataBaseAttributes
{
    class PendingOrder
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public bool IsProcessed { get; set; }

        public string OrderType { get; set; }

        public int OrderTypeId { get; set; }

        public bool IsMovePickupComplete { get; set; }
    }
}
