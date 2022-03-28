using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblUnadjustedForecast
    {
        public int AdjId { get; set; }
        public int? LocationId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? PerDiem { get; set; }
        public decimal? NonPerDiem { get; set; }
        public decimal? Utilization { get; set; }
        public int? PatientDays { get; set; }
        public DateTime? Inserted { get; set; }
        public int? HospiceId { get; set; }
        public int? PatientId { get; set; }
        public int? CustomerId { get; set; }
        public int? NoRevenue { get; set; }
    }
}
