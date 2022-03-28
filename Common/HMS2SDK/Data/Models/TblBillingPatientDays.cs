using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblBillingPatientDays
    {
        public int PatientdaysId { get; set; }
        public int? CustomerId { get; set; }
        public int? HospiceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? PatientDays { get; set; }
    }
}
