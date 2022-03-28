using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class PurchaseOrder
    {
        [JsonProperty("internalId")]
        public int NetSuitePurchaseOrderId { get; set; }

        [JsonProperty("ponum")]
        public int PurchaseOrderNumber { get; set; }

        public string VendorName { get; set; }

        [JsonProperty("vendor")] 
        public int VendorNumber { get; set; }

        public DateTime DateCreated { get; set; }

        [JsonProperty("status")]
        public string PurchaseOrderStatus { get; set; }

        public IEnumerable<Receipt> Receipts { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<NetSuiteInventoryLineItem> PurchaseOrderLineItems { get; set; }

        [JsonProperty("locationId")]
        public int SiteId { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}
