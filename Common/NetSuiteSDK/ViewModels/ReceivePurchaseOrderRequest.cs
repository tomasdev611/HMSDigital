using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ReceivePurchaseOrderRequest
    {
        [JsonProperty("poInternalId")]
        public int NetSuitePurchaseOrderId { get; set; }

        [JsonProperty("imageUrls")]
        public IEnumerable<string> ImageUrls { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<PurchaseOrderLineItem> ItemLines { get; set; }
    }
}
