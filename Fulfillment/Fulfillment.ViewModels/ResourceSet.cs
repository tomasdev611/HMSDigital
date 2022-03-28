using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class ResourceSet
    {
        public int EstimatedTotal { get; set; }
        
        public List<Resource> Resources { get; set; }
    }
}
