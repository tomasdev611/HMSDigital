using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AddNetSuiteInventoryRequest
    {
        [JsonProperty("siteId")]
        public int NetSuiteSiteId { get; set; }

        [JsonProperty("items")]
        public IEnumerable<NetSuiteInventoryLineRequest> Items { get; set;}
    }
}
