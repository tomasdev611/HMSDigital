using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblInvoices
    {
        public int InvoiceId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PerDiem { get; set; }
        public decimal? NonPerDiem { get; set; }
        public decimal? Utilization { get; set; }
        public int? PatientDays { get; set; }
        public string Filename { get; set; }
        public DateTime? Timestamp { get; set; }
        public string DatemonthPrefix { get; set; }
        public int? CustomerId { get; set; }
        public string CombinedInv { get; set; }
    }
}
