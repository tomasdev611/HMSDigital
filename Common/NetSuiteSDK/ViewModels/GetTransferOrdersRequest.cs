using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class GetTransferOrdersRequest
    {
        [JsonProperty("siteId")]
        public int NetSuiteLocationId { get; set; }

        [JsonProperty("truckTransferOrders")]
        public bool TruckTransferOrders { get; set; }
    }
}
