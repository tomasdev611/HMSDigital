using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HMSDigital.Patient.FHIR.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FhirModel = Hl7.Fhir.Model;
using PatientOrderRequest = HMSDigital.Patient.ViewModels.PatientOrderRequest;

namespace HMSDigital.Patient.FHIR.BusinessLayer.Interfaces
{
    public interface IFHIRService
    {
        Task<PaginatedList<FHIRPatientDetail>> GetAllPatients();

        Task<FHIRPatientDetail> GetPatientById(string patientId);

        Task<FHIRPatientDetail> CreatePatient(FHIRPatientDetail patientDetail);

        Task<FhirModel.Organization> CreateOrganization(FHIRHospice hospice);

        Task<FhirModel.Organization> UpdateOrganization(FHIRHospice hospice);

        Task<FHIRPatientDetail> UpdatePatient(Guid patientId, FHIRPatientDetail patientDetail);

        Task RecordPatientOrder(PatientOrderRequest patientOrderRequest);

        Task DeletePatient(string patientId);

        Task<IEnumerable<FHIRPatientDetail>> GetPatientsByIds(IEnumerable<Guid> ids);

        Task<IEnumerable<FHIRPatientDetail>> SearchPatientsByYearOfBirth(int patientYearOfBirth);

        Task<IEnumerable<FHIRPatientDetail>> SearchPatientsByBirthDate(DateTime patientDateOfBirth);

        Task<IEnumerable<FHIRPatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest);
    }
}
