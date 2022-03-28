
namespace HMS2SDK.Data.Models
{
    public partial class TblPiIssues
    {
        public int PiissueId { get; set; }
        public int? LocationId { get; set; }
        public int? InventoryId { get; set; }
        public string IssueDescription { get; set; }
        public int? IssueLocationId { get; set; }
        public string AssetTag { get; set; }
    }
}
