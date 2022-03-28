using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class ReceivePurchaseOrderResponse
    {
        [JsonProperty("internalId")]
        public int InternalId { get; set; }

        [JsonProperty("ponum")]
        public int PurchaseOrderNumber { get; set; }

        [JsonProperty("vendorName")]
        public string VendorName { get; set; }

        [JsonProperty("vendor")]
        public int Vendor { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("receipts")]
        public List<ReceiptModel> Receipts { get; set; }

        [JsonProperty("itemLines")]
        public List<PurchaseOrderLineItemModel> LineItems { get; set; }

        [JsonProperty("locationId")]
        public int LocationId { get; set; }

        [JsonProperty("imageUrls")]
        public List<string> ImageUrls { get; set; }
    }
}
