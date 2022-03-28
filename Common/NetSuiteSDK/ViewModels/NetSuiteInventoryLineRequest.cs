using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteInventoryLineRequest
    {
        [JsonProperty("itemId")]
        public int NetSuiteItemId { get; set; }

        [JsonProperty("quantityToAdd")]
        public int QuantityToAdd { get; set; }

        [JsonProperty("inventory")]
        public IEnumerable<NetSuiteInventory> Inventory { get; set; }
    }
}
