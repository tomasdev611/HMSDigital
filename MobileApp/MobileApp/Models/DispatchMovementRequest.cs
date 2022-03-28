using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class DispatchMovementRequest
    {
        public int RequestId { get; set; }

        public string RequestType { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool AllowPartialDispatch { get; set; }

        public int VehicleId { get; set; }

        public IEnumerable<DispatchItems> DispatchItems { get; set; }
    }
}
