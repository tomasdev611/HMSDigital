using System;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientLinkRequest
    {
        public Guid PatientUuid { get; set; }

        public int Hms2Id { get; set; }
    }
}
