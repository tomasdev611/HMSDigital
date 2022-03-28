using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Resource
    {
        public string __type { get; set; }
        
        public List<AgentItinerary> AgentItineraries { get; set; }
        
        public bool IsAccepted { get; set; }
        
        public bool IsCompleted { get; set; }
        
        public List<object> UnscheduledItems { get; set; }
    }

}
