using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class FulfillReceiveOrderResponse
    {
        [JsonProperty("transferOrderId")]
        public int TransferOrderId { get; set; }

        [JsonProperty("netSuiteTransferOrderId")]
        public int NetSuiteTransferOrderId { get; set; }

        [JsonProperty("transferOrderStatus")]
        public string TransferOrderStatus { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("sourceLocation")]
        public SourceLocation SourceLocation { get; set; }

        [JsonProperty("destinationLocation")]
        public SourceLocation DestinationLocation { get; set; }

        [JsonProperty("orderLineItems")]
        public List<PurchaseOrderLineItemModel> OrderLineItems { get; set; }
    }
}
