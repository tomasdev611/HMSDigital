using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public DateTime OrderDateTime { get; set; }

        public int ShippingAddressId { get; set; }

        public int OrderTypeId { get; set; }

        public int StatusId { get; set; }

        public int DispatchStatusId { get; set; }

        public string OrderType { get; set; }

        public string OrderStatus { get; set; }

        public string DispatchStatus { get; set; }

        /// <summary>
        /// Ordering nurse
        /// </summary>
        public int OrderRecipientUserId { get; set; }

        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public Guid PatientUuid { get; set; }

        public int SiteId { get; set; }

        public bool StatOrder { get; set; }

        public int NetSuiteOrderId { get; set; }

        public string OrderNumber { get; set; }

        public string ConfirmationNumber { get; set; }

        public DateTime RequestedStartDateTime { get; set; }

        public DateTime RequestedEndDateTime { get; set; }

        public string ExternalOrderNumber { get; set; }

        public string PickupReason { get; set; }

        public Hospice Hospice { get; set; }

        public Address DeliveryAddress { get; set; }

        public Address PickupAddress { get; set; }

        public DateTime FulfillmentStartDateTime { get; set; }

        public DateTime FulfillmentEndDateTime { get; set; }

        public IEnumerable<OrderFulfillmentLineItem> OrderFulfillmentLineItems { get; set; }

        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }

        public IEnumerable<OrderNote> OrderNotes { get; set; }

        public string OrderingNurse { get; set; }

        public string CreatedByUser { get; set; }

        public string AssignedDriver { get; set; }

        public string ModifiedByUser { get; set; }

        public bool IsExceptionFulfillment { get; set; }

        public int? HospiceMemberId { get; set; }
    }
}
