using System;
using System.Collections.Generic;
using System.Text;
using MobileApp.Models;

namespace MobileApp.Models
{
    public class VehicleLoadlist
    {
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public IEnumerable<LoadlistItem> Items { get; set; }

        public IEnumerable<OrderHeader> Orders { get; set; }

        public int TotalItemCount { get; set; }

        public int TotalInventoryCount { get; set; }
    }
}
