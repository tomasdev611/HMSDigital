using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryTransfers6102015
    {
        public int InvtransId { get; set; }
        public int? LocationIdFrom { get; set; }
        public int? LocationIdTo { get; set; }
        public int? InvStatus { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public int? UserIdProcessor { get; set; }
        public string ApprovalNotes { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? TransferModality { get; set; }
        public decimal? FreightCost { get; set; }
        public int? FreightPkg { get; set; }
        public int? FreightPkgNum { get; set; }
        public int? FreightPkgLength { get; set; }
        public int? FreightPkgWidth { get; set; }
        public int? FreightPkgHeight { get; set; }
    }
}
