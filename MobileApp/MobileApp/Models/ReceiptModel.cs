using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class ReceiptModel
    {
        public DateTime DateCreated { get; set; }

        public int ReceiptId { get; set; }

        public IEnumerable<PurchaseOrderLineItemModel> ItemLines { get; set; }
    }
}