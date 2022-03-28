using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSInventoryLineRequest
    {
        public int ItemId { get; set; }

        public int QuantityToAdd { get; set; }

        public IEnumerable<NSInventoryItem> Inventory { get; set; }
    }
}
