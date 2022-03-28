using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class FulfillReceiveOrderRequest
    {
        [JsonProperty("poInternalId")]
        public int NetSuitePurchaseOrderId { get; set; }

        [JsonProperty("isFulfill")]
        public bool IsFulfill { get; set; }

        [JsonProperty("orderLineItems")]
        public IEnumerable<PurchaseOrderLineItemModel> OrderItemLines { get; set; }
    }
}
