using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchResponse
    {
        public IEnumerable<RouteItem> Routes { get; set; }

        public IEnumerable<OrderHeader> OrderHeaders { get; set; }

        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public int DriverId { get; set; }

        public Driver Driver { get; set; }
    }
}
