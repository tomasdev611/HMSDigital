using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class ItineraryItem : ItineraryItemBase
    {
        public DateTime ClosingTime { get; set; }

        public string DwellTime { get; set; }
    }
}
