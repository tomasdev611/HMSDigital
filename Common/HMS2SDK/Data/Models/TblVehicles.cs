using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblVehicles
    {
        public int VehicleId { get; set; }
        public int? LocationCode { get; set; }
        public string VehicleUnitId { get; set; }
        public string VinNumber { get; set; }
        public string Year { get; set; }
        public string MakeModel { get; set; }
        public string License { get; set; }
        public string State { get; set; }
        public DateTime? ExpDate { get; set; }
    }
}
