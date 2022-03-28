using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryIssues
    {
        public int InvissueId { get; set; }
        public int? LocationId { get; set; }
        public int? PatientId { get; set; }
        public int? InventoryId { get; set; }
        public int? OrderId { get; set; }
        public string InvCode { get; set; }
        public int? AssetTag { get; set; }
        public DateTime? DeliveredTimestamp { get; set; }
        public int? DriverId { get; set; }
        public int? Delivery { get; set; }
        public int? Pickup { get; set; }
        public string DispositionNotes { get; set; }
        public int Status { get; set; }
        public DateTime? PickupTimestamp { get; set; }
        public int? LinkId { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public string IssueDescription { get; set; }
    }
}
