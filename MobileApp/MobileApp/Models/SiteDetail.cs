using System;
namespace MobileApp.Models
{
    public class SiteDetail
    {
        public string Address { get; set; }

        public string ETA { get; set; }

        public string SiteAdmin { get; set; }

        public string ContactNumber { get; set; }

        public int DeliveryTime { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int DeliveryCount { get; set; }

        public int ItemsCount { get; set; }
    }
}
