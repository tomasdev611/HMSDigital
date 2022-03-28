using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class TransferOrderCreateRequest
    {
        public int SourceLocationId { get; set; }

        public int DestinationLocationId { get; set; }

        public IEnumerable<PurchaseOrderLineItemModel> OrderLineItems { get; set; }
    }
}
