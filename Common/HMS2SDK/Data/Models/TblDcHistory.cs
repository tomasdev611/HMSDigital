using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblDcHistory
    {
        public int DcId { get; set; }
        public int? PatientId { get; set; }
        public int? DcDate { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
