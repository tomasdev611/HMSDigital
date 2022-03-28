
namespace HMS2SDK.Data.Models
{
    public partial class TblOffCapSummary
    {
        public int OffcapId { get; set; }
        public int? InventoryId { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
        public decimal? OffCapCost { get; set; }
        public string OffCapMonth { get; set; }
        public string OffCapYear { get; set; }
        public int? OrderedBy { get; set; }
        public int? NumDays { get; set; }
        public int? OffCapApprovalUserId { get; set; }
        public int? AutoApproval { get; set; }
        public int? OrderId { get; set; }
    }
}
