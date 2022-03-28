using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPatientUpdates
    {
        public int UpdateId { get; set; }
        public int? PatientId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
