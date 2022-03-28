using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class DriverItineraryResponse
    {
        public DriverItineraryResponse()
        {
            Driver = new Driver();
            Instructions = new List<InstructionResponse>();
            Route = new Route();
        }

        public Driver Driver { get; set; }

        public IEnumerable<InstructionResponse> Instructions { get; set; }

        public Route Route { get; set; }
    }
}
