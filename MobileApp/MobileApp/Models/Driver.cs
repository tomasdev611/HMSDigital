using System.Collections.Generic;

namespace MobileApp.Models
{
    public class Driver : User
    {      
        public int Id { get; set; }

        public string VehicleId { get; set; }

        public int? CurrentVehicleId { get; set; }

        public Vehicle CurrentVehicle { get; set; }

        public int? CurrentSiteId { get; set; }

        public string CurrentSiteName { get; set; }
    
    }
}
