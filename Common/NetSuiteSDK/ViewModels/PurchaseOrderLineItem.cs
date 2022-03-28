using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class PurchaseOrderLineItem
    {
        [JsonProperty("itemId")]
        public int NetSuiteItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        [JsonProperty("isSerial")]
        public bool IsSerial { get; set; }

        [JsonProperty("inventory")]
        public IEnumerable<NetSuiteInventory> LineItemInventory { get; set; }
    }
}
