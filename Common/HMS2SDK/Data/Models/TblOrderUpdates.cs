using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblOrderUpdates
    {
        public int OrderupdateId { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Delivery { get; set; }
        public int? Pickup { get; set; }
        public int? Cancel { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OffCapApproval { get; set; }
        public int? OffCapCancel { get; set; }
        public int? OffCapAutoApproval { get; set; }
        public string CancelNotes { get; set; }
        public string OffcapComments { get; set; }
        public int? IncDelivery { get; set; }
        public int? TransitionOrder { get; set; }
        public int? BopOrder { get; set; }
    }
}
