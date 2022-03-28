using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class TransferOrder
    {
        public int TransferOrderId { get; set; }

        public int NetSuiteTransferOrderId { get; set; }

        public string TransferOrderStatus { get; set; }

        public DateTime DateCreated { get; set; }

        public Site SourceLocation { get; set; }

        public Site DestinationLocation { get; set; }

        public IEnumerable<InventoryLineItem> OrderLineItems { get; set; }
    }
}
