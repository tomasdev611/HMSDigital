using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class Inventory : InventoryRequest
    {
        public int Id { get; set; }

        public Item Item { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public int NetSuiteInventoryId { get; set; }

    }
}
