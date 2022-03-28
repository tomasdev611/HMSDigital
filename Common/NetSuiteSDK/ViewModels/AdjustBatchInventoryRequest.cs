using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustBatchInventoryRequest
    {
        [JsonProperty("items")]
        public IEnumerable<AdjustInventoryRequest> Items { get; set; }
    }
}
