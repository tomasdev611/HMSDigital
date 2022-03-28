using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderLineItems
    {
        public OrderLineItems()
        {
            OrderFulfillmentLineItems = new HashSet<OrderFulfillmentLineItems>();
            PatientInventory = new HashSet<PatientInventory>();
        }

        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int? SiteId { get; set; }
        public int DeliveryAddressId { get; set; }
        public int? PickupAddressId { get; set; }
        public int? ItemId { get; set; }
        public int ItemCount { get; set; }
        public DateTime? RequestedStartDateTime { get; set; }
        public DateTime? RequestedEndDateTime { get; set; }
        public int? NetSuiteOrderLineItemId { get; set; }
        public int? NetSuiteItemId { get; set; }
        public int? StatusId { get; set; }
        public int? DispatchStatusId { get; set; }
        public int? ActionId { get; set; }
        public string EquipmentSettings { get; set; }
        public string SerialNumber { get; set; }
        public string AssetTagNumber { get; set; }
        public string LotNumber { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? AdditionalField1 { get; set; }
        public decimal? AdditionalField2 { get; set; }
        public decimal? AdditionalField3 { get; set; }
        public DateTime? AdditionalField4 { get; set; }
        public decimal? AdditionalField5 { get; set; }
        public decimal? AdditionalField6 { get; set; }

        public virtual OrderTypes Action { get; set; }
        public virtual Users CreatedByUser { get; set; }
        public virtual Addresses DeliveryAddress { get; set; }
        public virtual OrderLineItemStatusTypes DispatchStatus { get; set; }
        public virtual Items Item { get; set; }
        public virtual OrderHeaders OrderHeader { get; set; }
        public virtual Addresses PickupAddress { get; set; }
        public virtual Sites Site { get; set; }
        public virtual OrderLineItemStatusTypes Status { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
        public virtual ICollection<PatientInventory> PatientInventory { get; set; }
    }
}
