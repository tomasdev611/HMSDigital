using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class InventoryLineItem
    {
        public int ItemId { get; set; }

        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        public int QuantityShipped { get; set; }

        public int QuantityReceived { get; set; }

        public bool IsSerial { get; set; }

        public IEnumerable<InventoryRequest> Inventory { get; set; }
    }
}
