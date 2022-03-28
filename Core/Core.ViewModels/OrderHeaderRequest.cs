using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class OrderHeaderRequest
    {
        public int Id { get; set; }

        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public int? HospiceMemberId { get; set; }

        public Guid PatientUuid { get; set; }

        public int OrderTypeId { get; set; } 

        public int OrderStatusId { get; set; }

        public IEnumerable<UpdateOrderNotesRequest> OrderNotes { get; set; }

        public bool StatOrder { get; set; }

        public DateTime RequestedStartDateTime { get; set; }

        public DateTime RequestedEndDateTime { get; set; }

        public string ConfirmationNumber { get; set; }

        public string ExternalOrderNumber { get; set; }

        public string PickupReason { get; set; }

        public CoreAddressRequest DeliveryAddress { get; set; }

        public CoreAddressRequest PickupAddress { get; set; }

        public IEnumerable<CoreOrderLineItemRequest> OrderLineItems { get; set; }
    }
}
