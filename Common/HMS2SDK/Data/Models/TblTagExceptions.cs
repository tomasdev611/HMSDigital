
namespace HMS2SDK.Data.Models
{
    public partial class TblTagExceptions
    {
        public int ExceptionId { get; set; }
        public int? InventoryId { get; set; }
        public int? LocationId { get; set; }
        public int? OrderId { get; set; }
        public int? LinkId { get; set; }
        public string AssetTagDelivered { get; set; }
        public string InvCode { get; set; }
        public string AssetTagPickedup { get; set; }
        public string ExceptionType { get; set; }
    }
}
