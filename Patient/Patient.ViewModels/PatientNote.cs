using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientNote
    {
        public string Note { get; set; }

        public int CreatedByUserId { get; set; }

        public string CreatedByUserName{get;set;}

        public DateTime CreatedDateTime { get; set; }
    }
}
