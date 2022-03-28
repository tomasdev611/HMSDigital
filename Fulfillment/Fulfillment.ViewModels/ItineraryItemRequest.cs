using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class ItineraryItemRequest : ItineraryItemBase
    {
        [JsonProperty("expectedDeliveryDateTime")]
        public DateTime ClosingTime { get; set; }

        [JsonProperty("processingTime")]
        public string DwellTime { get; set; }
    }
}
