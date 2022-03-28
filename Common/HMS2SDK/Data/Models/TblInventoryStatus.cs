
namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryStatus
    {
        public int StatusId { get; set; }
        public int? InventoryId { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public int? Status { get; set; }
        public int? PatientId { get; set; }
    }
}
