using System.Collections.Generic;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Agent
    {
       public List<int> Capacity { get; set; }

        public string Name { get; set; }

        public List<Shift> Shifts { get; set; }
    }
}
