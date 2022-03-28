
namespace HMS2SDK.Data.Models
{
    public partial class TblLocationInvQty
    {
        public int InvcountId { get; set; }
        public int? LocationId { get; set; }
        public int? InventoryId { get; set; }
        public string AssetTag { get; set; }
        public int? QtyStatus { get; set; }
    }
}
