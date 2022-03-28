using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteInventoryLineItem
    {
        [JsonProperty("itemId")]
        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public int QuantityShipped { get; set; }

        public int QuantityReceived { get; set; }

        [JsonProperty("isSerial")]
        public bool IsSerial { get; set; }

        [JsonProperty("inventory")]
        public IEnumerable<NetSuiteInventory> Inventory { get; set; }
    }
}
