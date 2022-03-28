
namespace HMS2SDK.Data.Models
{
    public partial class TblOrderStats
    {
        public int SummaryId { get; set; }
        public int? SourceType { get; set; }
        public string SourceMonth { get; set; }
        public string SourceYear { get; set; }
        public string HospiceQuery { get; set; }
        public int? SourceCount { get; set; }
        public string ReportGeo { get; set; }
    }
}
