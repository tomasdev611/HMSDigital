
namespace HMS2SDK.Data.Models
{
    public partial class AssetTagMaster
    {
        public int AtagId { get; set; }
        public int? InventoryId { get; set; }
        public int? LocationId { get; set; }
        public int? PatientId { get; set; }
        public string AssetTag { get; set; }
        public int? ImproperTag { get; set; }
        public int? PiProcessed { get; set; }
        public int? PoProcessed { get; set; }
        public int? OtherProcessed { get; set; }
    }
}
