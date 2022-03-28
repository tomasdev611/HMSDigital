using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteTransferOrder
    {
        public int TransferOrderId { get; set; }

        [JsonProperty("internalId")]
        public int NetSuiteTransferOrderId { get; set; }

        [JsonProperty("status")]
        public string TransferOrderStatus { get; set; }

        public DateTime DateCreated { get; set; }

        [JsonProperty("sourceLocationId")]
        public int NetSuiteSourceLocationId { get; set; }

        [JsonProperty("destinationLocationId")]
        public int NetSuiteDestinationLocationId { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<NetSuiteInventoryLineItem> OrderLineItems { get; set; }
    }
}
