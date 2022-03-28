using Newtonsoft.Json;
using System;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class FulfilmentItem
    {
        [JsonProperty("netSuiteOrderLineItemId")]
        public int? NetSuiteOrderLineItemId { get; set; }

        [JsonProperty("netSuiteItemId")]
        public int NetSuiteItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("netSuiteWarehouseId")]
        public int NetSuiteWarehouseId { get; set; }

        [JsonProperty("deliveryDateTime")]
        public DateTime? DeliveryDateTime { get; set; }

        [JsonProperty("pickupDateTime")]
        public DateTime? PickupDateTime { get; set; }

        [JsonProperty("serviceNotes")]
        public string ServiceNotes { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("deliveredStatus")]
        public string DeliveredStatus { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("lotNumber")]
        public string LotNumber { get; set; }

        [JsonProperty("assetTag")]
        public string AssetTag { get; set; }

        /// <summary>
        /// Required only when DispatchOnly is true    
        /// </summary>
        [JsonProperty("pickupOrderDateTime")]
        public DateTime? PickupOrderDateTime { get; set; }   
    }
}
