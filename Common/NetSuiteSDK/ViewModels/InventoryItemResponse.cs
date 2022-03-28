using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class InventoryItemResponse
    {
        [JsonProperty("itemId")]
        public int NetsuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }
    }
}
