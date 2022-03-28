using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderFulfillmentLineItems
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int OrderLineItemId { get; set; }
        public int? NetSuiteWarehouseId { get; set; }
        public int? NetSuiteDispatchRecordId { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public Guid? PatientUuid { get; set; }
        public int? NetSuiteOrderId { get; set; }
        public int? NetSuiteItemId { get; set; }
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
        public string OrderType { get; set; }
        public string DeliveredStatus { get; set; }
        public bool IsFulfilmentConfirmed { get; set; }
        public DateTime? FulfillmentStartDateTime { get; set; }
        public decimal? FulfillmentStartAtLatitude { get; set; }
        public decimal? FulfillmentStartAtLongitude { get; set; }
        public DateTime? FulfillmentEndDateTime { get; set; }
        public decimal? FulfillmentEndAtLatitude { get; set; }
        public decimal? FulfillmentEndAtLongitude { get; set; }
        public int? FulfilledByVehicleId { get; set; }
        public int? FulfilledByDriverId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public bool? IsWebportalFulfillment { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Drivers FulfilledByDriver { get; set; }
        public virtual Sites FulfilledByVehicle { get; set; }
        public virtual Items Item { get; set; }
        public virtual OrderHeaders OrderHeader { get; set; }
        public virtual OrderLineItems OrderLineItem { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
