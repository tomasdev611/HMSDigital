using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class DriverBase : UserMinimal
    {
        public IEnumerable<int> SiteIds { get; set; }

        public IEnumerable<int> VehicleIds { get; set; }

        public int? CurrentVehicleId { get; set; }

        public int? CurrentSiteId { get; set; }
    }
}
