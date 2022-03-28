using HMSDigital.Common.ViewModels;
using System;

namespace HMSDigital.Core.ViewModels
{
    public class Driver : UserMinimal
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int VehicleId { get; set; }

        public string Network { get; set; }

        public string Division { get; set; }

        public int? CurrentVehicleId { get; set; }

        public Vehicle CurrentVehicle { get; set; }

        public int? CurrentSiteId { get; set; }

        public string CurrentSiteName { get; set; }

        public decimal LastKnownLatitude { get; set; }

        public decimal LastKnownLongitude { get; set; }

        public DateTime LocationUpdatedDateTime { get; set; }
    }
}
