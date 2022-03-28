using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPhysicalInventoryCompletedCopy
    {
        public int ComppiId { get; set; }
        public int? LocationId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
