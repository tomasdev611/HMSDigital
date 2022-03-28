using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public class OrderData : BindableObject
    {
        public int? SequenceNumber { get; set; }

        public string ContactPerson { get; set; }

        public int OrderID { get; set; }

        public bool IsCompleted { get; set; }

        public int ItemCount { get; set; }

        public long ContactNumber { get; set; }

        public string OrderNumber { get; set; }

        public string ExpectedDeliveryDate { get; set; }

        public string ETA { get; set; }

        public bool StatOrder { get; set; }

        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }

        public Address ShippingAddress { get; set; }

        public string OrderStatus { get; set; }

        public int StatusId { get; set; }

        public IEnumerable<NoteResponse> OrderNotes { get; set; }

        public Guid PatientUuId { get; set; }

        public IEnumerable<NoteResponse> PatientNotes { get; set; }

        public string OrderType { get; set; }

        private int _orderTypeId;
        public int OrderTypeId
        {
            get
            {
                return _orderTypeId;
            }
            set
            {
                _orderTypeId = value;
                OnPropertyChanged();
            }
        }

        public DateTime FulfillmentStartDateTime { get; set; }

        public decimal FulfillmentStartAtLatitude { get; set; }

        public decimal FulfillmentStartAtLongitude { get; set; }

        public bool IsInfectious { get; set; }

        public bool IsExceptionFulfillment { get; set; }
    }
}
