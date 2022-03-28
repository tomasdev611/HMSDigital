
namespace HMS2SDK.Data.Models
{
    public partial class TblCapex
    {
        public int Id { get; set; }
        public int? HospiceId { get; set; }
        public int? InventoryId { get; set; }
        public int? Invcount { get; set; }
        public decimal? Capexvalue { get; set; }
        public int? Groupnum { get; set; }
    }
}
