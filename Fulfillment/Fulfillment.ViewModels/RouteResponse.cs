using System;
using System.Collections.Generic;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class RouteResponse
    {
        public RouteResponse()
        {
            DriverItineraries = new List<DriverItineraryResponse>();
            UnAssignedItems = new List<object>();
        }

        public List<DriverItineraryResponse> DriverItineraries { get; set; }

        public List<object> UnAssignedItems { get; set; }
    }
}
