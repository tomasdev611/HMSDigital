using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class InventoryMovementRequest
    {
        [JsonProperty("netSuiteSourceWarehouseId")]
        public int NetSuiteSourceWarehouseId { get; set; }

        [JsonProperty("netSuiteDestinationWarehouseId")]
        public int NetSuiteDestinationWarehouseId { get; set; }

        [JsonProperty("items")]
        public IEnumerable<MovementItem> Items { get; set; }
    }
}
