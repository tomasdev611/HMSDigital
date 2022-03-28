using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class OptimizeRouteRequest
    {
        public IEnumerable<Agent> Agents { get; set; }

        public IEnumerable<ItineraryItem> ItineraryItems{ get; set; }
    }
}
