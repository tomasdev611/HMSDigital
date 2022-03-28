
namespace HMS2SDK.Data.Models
{
    public partial class TblUnadjusted
    {
        public int AdjId { get; set; }
        public int? LocationId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? PerDiem { get; set; }
        public decimal? NonPerDiem { get; set; }
        public decimal? Utilization { get; set; }
        public int? PatientDays { get; set; }
        public string Filename { get; set; }
    }
}
