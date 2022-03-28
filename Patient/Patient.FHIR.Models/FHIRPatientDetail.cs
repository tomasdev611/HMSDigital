using HMSDigital.Patient.ViewModels;
using System;

namespace HMSDigital.Patient.FHIR.Models
{
    public class FHIRPatientDetail : PatientDetail
    {
        public Guid? FhirPatientId { get; set; }

        public Guid? FhirOrganizationId { get; set; }
    }
}
