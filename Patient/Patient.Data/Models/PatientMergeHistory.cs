using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PatientMergeHistory
    {
        public int Id { get; set; }
        public Guid PatientUuid { get; set; }
        public Guid DuplicatePatientUuid { get; set; }
        public string ChangeLog { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
    }
}
