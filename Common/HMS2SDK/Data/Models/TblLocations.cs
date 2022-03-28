
namespace HMS2SDK.Data.Models
{
    public partial class TblLocations
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationZip { get; set; }
        public string LocationPhone { get; set; }
        public int Timezone { get; set; }
        public string LegacyMitsDivision { get; set; }
        public int? Vehicle { get; set; }
        public int? VehicleLocation { get; set; }
        public string Invemail { get; set; }
        public int? H2hRev { get; set; }
        public int? NoShowRevReport { get; set; }
        public int? ExcludeReports { get; set; }
        public string PccrEmailNotifications { get; set; }
        public string PccrAssignment { get; set; }
        public int? Inactive { get; set; }
        public int? NoPi { get; set; }
    }
}
