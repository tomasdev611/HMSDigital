using System;
using System.Collections.Generic;
using MobileApp.ViewModels;

namespace MobileApp.Models
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
