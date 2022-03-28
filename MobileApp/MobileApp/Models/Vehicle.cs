using System.Collections.Generic;

namespace MobileApp.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string SiteName { get; set; }

        public string CurrentDriverName { get; set; }

        public int CurrentDriverId { get; set; }

        public int? SiteId { get; set; }

        public string Vin { get; set; }

        public string Cvn { get; set; }

        public string Name { get; set; }

        public string LicensePlate { get; set; }
    }
}