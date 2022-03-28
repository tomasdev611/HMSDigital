using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchInventory
    {
        public Inventory Inventory { get; set; }

        public int Count { get; set; }

        public bool IsSerialized { get; set; }

        public int OrderLineItemId { get; set; }
    }
}
