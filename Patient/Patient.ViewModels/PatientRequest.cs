using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientRequest
    {
        public IEnumerable<int> PatientIds { get; set; }

        public IEnumerable<string> PatientUuids { get; set; }
    }
}
