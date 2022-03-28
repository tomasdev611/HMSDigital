using Newtonsoft.Json;
using System;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSInventory
    {
        [JsonProperty("internalId")]
        public int? NetSuiteInventoryId { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityAvailable { get; set; }

        public string SerialNumber { get; set; }


        [JsonProperty("LocationId")]
        public int? NetSuiteLocationId { get; set; }

        public string LotNumber { get; set; }

        public DateTime LotExpirationDate { get; set; }

        public string AssetTagNumber { get; set; }
    }
}
