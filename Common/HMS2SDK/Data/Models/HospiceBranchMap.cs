
namespace HMS2SDK.Data.Models
{
    public partial class HospiceBranchMap
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public string HmsName { get; set; }
        public int? SourceId { get; set; }
        public int? HmsId { get; set; }
    }
}
