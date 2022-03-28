using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class TOrderFulfillReceiveRequest
    {
        public int NetSuiteTransferOrderId { get; set; }

        public bool IsFulfill { get; set; }

        public IEnumerable<InventoryLineItem> OrderLineItems { get; set; }
    }
}
