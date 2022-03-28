
namespace HMS2SDK.Data.Models
{
    public partial class TblDrivers
    {
        public int DriverId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string VehicleId { get; set; }
        public string Mobile { get; set; }
        public string Network { get; set; }
        public string Division { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Inactive { get; set; }
        public int? LocationId { get; set; }
    }
}
