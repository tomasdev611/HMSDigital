using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ReceiptLineItem
    {
        [JsonProperty("itemId")]
        public int ItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        [JsonProperty("isSerial")]
        public bool IsSerial { get; set; }

        [JsonProperty("assetTags")]
        public IEnumerable<string> AssetTags { get; set; }
    }
}
