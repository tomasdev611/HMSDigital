using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPurchaseOrderDocuments
    {
        public int PodocId { get; set; }
        public int? PoId { get; set; }
        public string Filename { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? UserId { get; set; }
    }
}
