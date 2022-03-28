using HMSDigital.Common.ViewModels;
using System;

namespace HMSDigital.Patient.FHIR.Models
{
    public class FHIRHospice : Hospice
    {
        public Guid? FhirOrganizationId { get; set; }
    }
}
