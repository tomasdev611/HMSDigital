using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPurchaseOrders
    {
        public int PoId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? LocationId { get; set; }
        public int? VendorId { get; set; }
        public int? PoStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public int? UserId { get; set; }
        public int? PoClassSpecial { get; set; }
        public int? PoClassGrowth { get; set; }
        public int? PoClassMaintenance { get; set; }
        public int? PoClassBariatric { get; set; }
        public string PoClassComments { get; set; }
        public string PoPartner { get; set; }
        public int? PoClassification { get; set; }
    }
}
