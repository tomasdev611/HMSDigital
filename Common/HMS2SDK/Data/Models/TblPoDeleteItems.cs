using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPoDeleteItems
    {
        public int PodelId { get; set; }
        public int? PoitemId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
