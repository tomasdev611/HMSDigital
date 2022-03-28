using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Instruction : InstructionBase
    {
        public ItineraryItemRequest ItineraryItem { get; set; }
    }
}
