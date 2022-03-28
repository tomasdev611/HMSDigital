using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPurchaseOrderNotes
    {
        public int PonoteId { get; set; }
        public int? PoId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Note { get; set; }
    }
}
