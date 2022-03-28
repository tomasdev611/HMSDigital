using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class AgentItinerary
    {
        public Agent Agent { get; set; }
        
        public List<Instruction> Instructions { get; set; }
        
        public Route Route { get; set; }
    }
}
