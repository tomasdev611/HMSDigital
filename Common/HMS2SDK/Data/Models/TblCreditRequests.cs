using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblCreditRequests
    {
        public int CreditId { get; set; }
        public string PatientName { get; set; }
        public DateTime? BillingDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int? HospiceId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string PostArray { get; set; }
        public decimal? CreditAmount { get; set; }
    }
}
