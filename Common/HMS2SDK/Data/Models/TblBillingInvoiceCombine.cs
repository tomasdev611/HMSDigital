using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblBillingInvoiceCombine
    {
        public int CombineId { get; set; }
        public int? CustomerId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
