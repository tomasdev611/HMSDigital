using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteTOFulfillReceiveRequest
    {
        [JsonProperty("transferOrderInternalId")]
        public int NetSuiteTransferOrderId { get; set; }

        [JsonProperty("isFulfill")]
        public bool IsFulfill { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<NetSuiteInventoryLineItem> OrderLineItems { get; set; }
    }
}
