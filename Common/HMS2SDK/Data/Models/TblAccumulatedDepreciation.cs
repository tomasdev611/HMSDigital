using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblAccumulatedDepreciation
    {
        public int AccdepId { get; set; }
        public int? DepmasterId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? DepDate { get; set; }
        public decimal? DepAmount { get; set; }
        public string DepType { get; set; }
        public decimal? BookValue { get; set; }
    }
}
