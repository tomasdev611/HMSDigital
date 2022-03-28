using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class ItineraryItemBase
    {
        public List<object> DropOffFrom { get; set; }
       
        public Location Location { get; set; }
        
        public string Name { get; set; }
        
        public DateTime OpeningTime { get; set; }
        
        public int Priority { get; set; }
        
        public List<int> Quantity { get; set; }

        public bool Depot { get; set; }
    }
}
