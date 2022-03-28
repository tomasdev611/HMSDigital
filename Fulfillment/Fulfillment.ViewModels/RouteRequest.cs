using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class RouteRequest
    {
        public IEnumerable<Agent> Drivers { get; set; }

        [JsonProperty("orders")]
        public IEnumerable<ItineraryItemRequest> ItineraryItems{ get; set; }
    }
}
