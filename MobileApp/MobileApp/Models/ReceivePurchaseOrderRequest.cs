using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class ReceivePurchaseOrderRequest
    {
        [JsonProperty("poInternalId")]
        public int NetSuitePurchaseOrderId { get; set; }

        [JsonProperty("imageUrls")]
        public IEnumerable<string> ImageUrls { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<PurchaseOrderLineItemModel> ItemLines { get; set; }
    }
}   