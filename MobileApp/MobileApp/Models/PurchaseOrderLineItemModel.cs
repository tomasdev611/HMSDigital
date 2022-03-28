using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class PurchaseOrderLineItemModel
    {
        [JsonProperty("itemId")]
        public int ItemId { get; set; }

        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public int QuantityShipped { get; set; }

        [JsonProperty("quantityRecieved")]
        public int QuantityRecieved { get; set; }

        [JsonProperty("isSerial")]
        public bool IsSerial { get; set; }

        [JsonProperty("assetTags")]
        public IEnumerable<string> AssetTags { get; set; }

        public IEnumerable<InventoryRequest> Inventory { get; set; }

        [JsonIgnore]
        public bool IsPartialReceived => QuantityRecieved < Quantity;

        [JsonIgnore]
        public bool IsEdited { get; set; }

        [JsonIgnore]
        public bool IsReceived => !IsPartialReceived && QuantityRecieved == Quantity;

        [JsonIgnore]
        public string Status => GetStatus();

        [JsonIgnore]
        public IEnumerable<AssetTagsModel> AssetTagsModels { get; set; }

        private string GetStatus()
        {
            if(IsReceived)
            {
                return "Received";
            }
            else
            {
                return "";
            }    
        }
    }
}