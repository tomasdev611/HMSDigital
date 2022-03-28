using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class InventoryStatusTypes
    {
        public InventoryStatusTypes()
        {
            Inventory = new HashSet<Inventory>();
            PatientInventory = new HashSet<PatientInventory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<PatientInventory> PatientInventory { get; set; }
    }
}
