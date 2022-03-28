using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryWriteOffs
    {
        public int InvwoId { get; set; }
        public int? LocationId { get; set; }
        public string InvCode { get; set; }
        public int? InvStatus { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string AssetAge { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserId { get; set; }
        public string Reason { get; set; }
        public int? WoStatus { get; set; }
        public int? UserIdProcessor { get; set; }
        public string ApprovalNotes { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string Filename { get; set; }
        public string PatientName { get; set; }
        public string Hospice { get; set; }
    }
}
