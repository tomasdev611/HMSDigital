using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblUnadjustedForecastCopy
    {
        public int AdjId { get; set; }
        public int? LocationId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? PerDiem { get; set; }
        public decimal? NonPerDiem { get; set; }
        public decimal? Utilization { get; set; }
        public int? PatientDays { get; set; }
        public int? AllCustomers { get; set; }
        public int? OdyOnly { get; set; }
        public int? OdyExcluded { get; set; }
        public int? V2 { get; set; }
        public DateTime? Inserted { get; set; }
    }
}
