using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteInventory
    {
        [JsonProperty("assetTag")]
        public string AssetTagNumber { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
    }
}
