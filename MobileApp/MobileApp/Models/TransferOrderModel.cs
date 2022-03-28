using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class TransferOrder
    {
        public int TransferOrderId { get; set; }

        public int NetSuiteTransferOrderId { get; set; }

        public string TransferOrderStatus { get; set; }

        public DateTime DateCreated { get; set; }

        [JsonProperty("sourceLocation")]
        public SourceLocation SourceLocationItem { get; set; }

        public SourceLocation DestinationLocation { get; set; }

        public IEnumerable<PurchaseOrderLineItemModel> OrderLineItems { get; set; }

        [JsonIgnore]
        public int TotalQuantity => OrderLineItems?.Count() ?? 0;

        [JsonIgnore]
        public string LocationName => SourceLocationItem != null ? SourceLocationItem.FullAddress : DestinationLocation?.FullAddress;

        [JsonIgnore]
        public string StatusDescription => GetDescription();

        /// <summary>
        /// Get the Description for Status
        /// </summary>
        /// <returns></returns>
        private string GetDescription()
        {
            if (String.IsNullOrEmpty(TransferOrderStatus))
            {
                return "";
            }

            switch (TransferOrderStatus.ToLower())
            {
                case "pendingfulfillment":
                    return "Pending Fulfillment";
                case "partial":
                    return "Partial Fulfillment";
                case "pendingreceive":
                    return "Pending Receive";
                default:
                    return TransferOrderStatus;
            }
        }
    }

    public class SourceLocation
    {
        public int SiteCode { get; set; }
        public Address Address { get; set; }
        public string LocationType { get; set; }
        public List<SitePhoneNumber> SitePhoneNumber { get; set; }
        public bool IsDisable { get; set; }
        public object Vehicles { get; set; }
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Cvn { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public double Length { get; set; }
        public double Capacity { get; set; }
        public bool IsActive { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string CurrentDriverName { get; set; }
        public int CurrentDriverId { get; set; }
        public int NetSuiteLocationId { get; set; }
        public int ParentNetSuiteLocationId { get; set; }

        public string FullAddress => Address != null ? $"{Address.AddressLine1} : {Address.City}, {Address.State}" : Name;
    }
}
