using System;
using System.Collections.Generic;

namespace HMS2SDK.Data.Models
{
    public partial class InventoryPmcs
    {
        public InventoryPmcs()
        {
            InventoryPmcsLog = new HashSet<InventoryPmcsLog>();
        }

        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastMaintenance { get; set; }

        public virtual ICollection<InventoryPmcsLog> InventoryPmcsLog { get; set; }
    }
}
