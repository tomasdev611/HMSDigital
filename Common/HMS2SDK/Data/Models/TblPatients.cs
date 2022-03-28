using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPatients
    {
        public int PatientId { get; set; }
        public string PatientFirst { get; set; }
        public string PatientLast { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZip { get; set; }
        public string HospiceBillingId { get; set; }
        public DateTime? CreatedTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
    }
}
