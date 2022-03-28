using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchMovementRequest
    {
        public int RequestId { get; set; }

        public string RequestType { get; set; } // loadlist or TransferRequest

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool AllowPartialDispatch { get; set; }

        public int VehicleId { get; set; }

        public IEnumerable<DispatchItem> DispatchItems { get; set; }
    }
}
