using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblAfterHours
    {
        public int AhId { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
