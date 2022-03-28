using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblDeclinedOffCap
    {
        public int DenyId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
