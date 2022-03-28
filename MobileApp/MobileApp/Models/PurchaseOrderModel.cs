using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class PurchaseOrderModel
    {
        public string VendorName { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalQuantity { get; set; }
        public int LocationId { get; set; }

        [JsonProperty("internalId")]
        public int InternalId { get; set; }

        [JsonProperty("receipts")]
        public IEnumerable<ReceiptModel> Receipts { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("itemLines")]
        public IEnumerable<PurchaseOrderLineItemModel> PurchaseOrderLineItems { get; set; }

        [JsonProperty("vendor")]
        public int VendorNumber { get; set; }

        [JsonProperty("ponum")]
        public int PurchaseOrderNumber { get; set; }

        [JsonProperty("imageUrls")]
        public IEnumerable<string> Images { get; set; }

        #region Custom Properties

        [JsonIgnore]
        public string FullVendor => $"{VendorNumber} {VendorName}";

        [JsonIgnore]
        public bool IsPartialReceipt => Status?.ToUpper() == PurchaseOrderStatus.PARTIAL.ToString();

        [JsonIgnore]
        public string StatusDescription => GetDescription();

        #endregion

        /// <summary>
        /// Get the Description for Status
        /// </summary>
        /// <returns></returns>
        private string GetDescription()
        {
            if(String.IsNullOrEmpty(Status))
            {
                return "";
            }

            switch (Status.ToLower())
            {
                case "open":
                    return "Pending Receipt";
                case "partial":
                    return "Partial Receipt";
                case "received":
                    return "Received";
                default:
                    return "Pending Receipt";
            }
        }
    }

    public enum PurchaseOrderStatus
    {
        OPEN,
        PARTIAL,
        RECEIVED
    }
}