using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Route
    {
        public Location EndLocation { get; set; }

        public DateTime EndTime { get; set; }

        public Location StartLocation { get; set; }

        public DateTime StartTime { get; set; }

        public int TotalTravelDistance { get; set; }

        public string TotalTravelTime { get; set; }

        public List<Location> WayPoints { get; set; }
    }

}
