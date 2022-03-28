using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ReceivePurchaseOrderResponse
    {
        public bool Success { get; set; }

        public PurchaseOrder PurchaseOrder {get; set;}

        public ErrorResponse Message { get; set; }
    }
}
