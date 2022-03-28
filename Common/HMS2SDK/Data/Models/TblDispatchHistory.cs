using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblDispatchHistory
    {
        public int DhId { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? DispatchStatus { get; set; }
        public int? DriverId { get; set; }
    }
}
