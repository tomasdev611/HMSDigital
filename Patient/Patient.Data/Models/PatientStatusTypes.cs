using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PatientStatusTypes
    {
        public PatientStatusTypes()
        {
            PatientDetailsStatus = new HashSet<PatientDetails>();
            PatientDetailsStatusReason = new HashSet<PatientDetails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public virtual ICollection<PatientDetails> PatientDetailsStatus { get; set; }
        public virtual ICollection<PatientDetails> PatientDetailsStatusReason { get; set; }
    }
}
