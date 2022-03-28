using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class OrderFulfillmentRequest
    {
        public int OrderId { get; set; }

        public int? DriverId { get; set; }

        public int? VehicleId { get; set; }

        public DateTime FulfillmentStartDateTime { get; set; }

        public decimal FulfillmentStartAtLatitude { get; set; }

        public decimal FulfillmentStartAtLongitude { get; set; }

        public DateTime FulfillmentEndDateTime { get; set; }

        public decimal FulfillmentEndAtLatitude { get; set; }

        public decimal FulfillmentEndAtLongitude { get; set; }

        public IEnumerable<FulfillmentItem> FulfillmentItems { get; set; }

        public bool? IsWebportalFulfillment { get; set; }

        public string PartialFulfillmentReason { get; set; }

        public bool IsExceptionFulfillment { get; set; }
    }
}
