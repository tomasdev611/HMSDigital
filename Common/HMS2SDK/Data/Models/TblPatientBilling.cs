using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPatientBilling
    {
        public int PatientbillingId { get; set; }
        public int? PatientId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? BillingSaleDate { get; set; }
        public string BillingItem { get; set; }
        public decimal? BillingPurchaseAmt { get; set; }
        public decimal? BillingSaleAmt { get; set; }
        public string BillingNote { get; set; }
        public DateTime? BillingSubmitted { get; set; }
        public int? UserId { get; set; }
    }
}
