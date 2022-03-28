using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Shift
    {
        public Location EndLocation { get; set; }

        public DateTime EndTime { get; set; }

        public Location StartLocation { get; set; }

        public DateTime StartTime { get; set; }
    }
}
