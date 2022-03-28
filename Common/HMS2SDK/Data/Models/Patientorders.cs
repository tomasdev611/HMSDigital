using System;

namespace HMS2SDK.Data.Models
{
    public partial class Patientorders
    {
        public int? Completed { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Instructions { get; set; }
        public DateTime LastChanged { get; set; }
        public int? OrderedBy { get; set; }
        public int? PatientId { get; set; }
        public int Id { get; set; }
    }
}
