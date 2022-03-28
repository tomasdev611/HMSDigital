using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public DateTime OrderDateTime { get; set; }

        public int OrderTypeId { get; set; }

        public string OrderType { get; set; }

        public string OrderStatus { get; set; }

        public int StatusId { get; set; }

        public string Instruction { get; set; }

        public string OrderNumber { get; set; }

        public Guid PatientUuid { get; set; }

        public int SiteId { get; set; }

        public bool StatOrder { get; set; }

        public DateTime RequestedStartDateTime { get; set; }

        public DateTime RequestedEndDateTime { get; set; }

        public Address DeliveryAddress { get; set; }

        public Address PickupAddress { get; set; }

        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }

        public IEnumerable<OrderFulfillmentLineItem> OrderFulfillmentLineItems { get; set; }

        public IEnumerable<NoteResponse> OrderNotes { get; set; }

        public bool IsExceptionFulfillment { get; set; }
    }
}
