using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblLoginHistory
    {
        public int HistoryId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? UserId { get; set; }
        public string IpAddress { get; set; }
    }
}
