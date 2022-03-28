using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PatientNotes
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByUserId { get; set; }

        public virtual PatientDetails Patient { get; set; }
    }
}
