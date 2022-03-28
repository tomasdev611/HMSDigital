using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class InstructionResponse : InstructionBase
    {
        [JsonProperty("orders")]
        public ItineraryItemRequest ItineraryItem { get; set; }
    }
}
