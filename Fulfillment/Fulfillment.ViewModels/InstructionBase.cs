using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class InstructionBase
    {
        public string Duration { get; set; }

        public DateTime EndTime { get; set; }

        public string InstructionType { get; set; }

        public DateTime StartTime { get; set; }

        public int? Distance { get; set; }
    }
}
