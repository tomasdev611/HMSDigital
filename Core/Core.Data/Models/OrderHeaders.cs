using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderHeaders
    {
        public OrderHeaders()
        {
            OrderFulfillmentLineItems = new HashSet<OrderFulfillmentLineItems>();
            OrderLineItems = new HashSet<OrderLineItems>();
            OrderNotes = new HashSet<OrderNotes>();
            PatientInventory = new HashSet<PatientInventory>();
        }

        public int Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int DeliveryAddressId { get; set; }
        public int? PickupAddressId { get; set; }
        public int OrderRecipientUserId { get; set; }
        public int? HospiceMemberId { get; set; }
        public int? HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }
        public int OrderTypeId { get; set; }
        public int StatusId { get; set; }
        public int? DispatchStatusId { get; set; }
        public int? SiteId { get; set; }
        public DateTime? RequestedStartDateTime { get; set; }
        public DateTime? RequestedEndDateTime { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public int? NetSuiteCustomerContactId { get; set; }
        public int? NetSuiteOrderId { get; set; }
        public Guid PatientUuid { get; set; }
        public string OrderNumber { get; set; }
        public string ConfirmationNumber { get; set; }
        public DateTime? FulfillmentStartDateTime { get; set; }
        public decimal? FulfillmentStartAtLatitude { get; set; }
        public decimal? FulfillmentStartAtLongitude { get; set; }
        public DateTime? FulfillmentEndDateTime { get; set; }
        public decimal? FulfillmentEndAtLatitude { get; set; }
        public decimal? FulfillmentEndAtLongitude { get; set; }
        public bool? StatOrder { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public string ExternalOrderNumber { get; set; }
        public string PickupReason { get; set; }
        public string PartialFulfillmentReason { get; set; }
        public bool IsExceptionFulfillment { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Addresses DeliveryAddress { get; set; }
        public virtual OrderHeaderStatusTypes DispatchStatus { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual HospiceLocations HospiceLocation { get; set; }
        public virtual HospiceMember HospiceMember { get; set; }
        public virtual OrderTypes OrderType { get; set; }
        public virtual Addresses PickupAddress { get; set; }
        public virtual Sites Site { get; set; }
        public virtual OrderHeaderStatusTypes Status { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual DispatchInstructions DispatchInstructions { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItems { get; set; }
        public virtual ICollection<OrderNotes> OrderNotes { get; set; }
        public virtual ICollection<PatientInventory> PatientInventory { get; set; }
    }
}
