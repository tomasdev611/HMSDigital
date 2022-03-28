using System;

namespace HospiceSource.Digital.Patient.SDK.ViewModels
{
    public class PatientStatusValidationResponse
    {
        public Guid PatientUuid { get; set; }

        public bool HasValidStatus { get; set; }
    }
}
