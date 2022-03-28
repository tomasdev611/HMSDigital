using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class TransferOrderCreateRequest
    {
        public int SourceLocationId { get; set; }

        public int DestinationLocationId { get; set; }

        public IEnumerable<InventoryLineItem> OrderLineItems { get; set; }
    }
}
