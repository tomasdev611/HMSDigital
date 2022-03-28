using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteInventoryResponse
    {
        [JsonProperty("inventoryId")]
        public int NetSuiteInventoryId { get; set; }

        [JsonProperty("itemId")]
        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTag { get; set; }
    }
}
