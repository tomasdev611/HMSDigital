using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryAdds772015
    {
        public int InvaddId { get; set; }
        public int? LocationId { get; set; }
        public int? InventoryId { get; set; }
        public string InvCode { get; set; }
        public int? InvStatus { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string AssetAge { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserId { get; set; }
        public int? RelatedId { get; set; }
        public int? AddStatus { get; set; }
        public int? UserIdProcessor { get; set; }
        public string ApprovalNotes { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? Processed { get; set; }
        public string AssetRejects { get; set; }
    }
}
