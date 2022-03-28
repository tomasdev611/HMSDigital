using System;

namespace HMS2SDK.Data.Models
{
    public partial class InventoryPmcsLog
    {
        public int Id { get; set; }
        public int InventoryPmcsId { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? OldLastMaintenance { get; set; }

        public virtual InventoryPmcs InventoryPmcs { get; set; }
    }
}
