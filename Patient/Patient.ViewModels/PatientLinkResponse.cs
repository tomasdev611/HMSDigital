using System;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientLinkResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public PatientDetail Patient { get; set; }

        public Guid PatientUuid { get; set; }
    }
}
