using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustInventoryRequest
    {
        [JsonProperty("itemSku")]
        public string ItemNumber { get; set; }

        [JsonProperty("netSuiteWarehouseId")]
        public int? NetSuiteWarehouseId { get; set; }

        [JsonProperty("serialOrLotNumber")]
        public string SerialOrLotNumber { get; set; }

        [JsonProperty("totalQuantityOnHand")]
        public int TotalQuantityOnHand { get; set; }

        [JsonProperty("assetTag")]
        public string AssetTagNumber { get; set; }
    }
}
