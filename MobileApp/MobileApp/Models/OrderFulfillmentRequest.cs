using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class OrderFulfillmentRequest
    {
        public int OrderId { get; set; }

        public DateTime FulfillmentStartDateTime { get; set; }

        public decimal FulfillmentStartAtLatitude { get; set; }

        public decimal FulfillmentStartAtLongitude { get; set; }

        public DateTime FulfillmentEndDateTime { get; set; }

        public decimal FulfillmentEndAtLatitude { get; set; }

        public decimal FulfillmentEndAtLongitude { get; set; }

        public IEnumerable<FulfillmentItems> FulfillmentItems { get; set; }

        public string PartialFulfillmentReason { get; set; }

        public bool IsExceptionFulfillment { get; set; }
    }
}
