using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class OrderFulfillmentLineItem
    {
        public int Id { get; set; }
        
        public int OrderHeaderId { get; set; }
        
        public int OrderLineItemId { get; set; }

        public string ItemName { get; set; }

        public int FulfilledBySiteId { get; set; }
        
        public int NetSuiteDispatchRecordId { get; set; }
        
        public int NetSuiteCustomerId { get; set; }
        
        public Guid PatientUuid { get; set; }
        
        public int NetSuiteOrderId { get; set; }
        
        public int NetSuiteItemId { get; set; }
        
        public int Quantity { get; set; }
        
        public string AssetTag { get; set; }
        
        public string LotNumber { get; set; }
        
        public string SerialNumber { get; set; }
        
        public string OrderType { get; set; }
        
        public string DeliveredStatus { get; set; }
        
        public bool IsFulfilmentConfirmed { get; set; }
        
        public DateTime FulfillmentStartDateTime { get; set; }
        
        public decimal FulfillmentStartAtLatitude { get; set; }
        
        public decimal FulfillmentStartAtLongitude { get; set; }
        
        public DateTime FulfillmentEndDateTime { get; set; }
        
        public decimal FulfillmentEndAtLatitude { get; set; }
        
        public decimal FulfillmentEndAtLongitude { get; set; }
        
        public int FulfilledByVehicleId { get; set; }

        public string FulfilledByVehicleCvn { get; set; }

        public int FulfilledByDriverId { get; set; }

        public string FulfilledByDriverName { get; set; }
    }
}
