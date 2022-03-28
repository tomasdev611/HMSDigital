using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using Sieve.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PatientOrderRequest = HMSDigital.Patient.ViewModels.PatientOrderRequest;

namespace HMSDigital.Patient.BusinessLayer.Interfaces
{
    public interface IPatientV2Service
    {
        Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel, bool ignoreFilter = false);

        Task<PatientDetail> GetPatientById(int patientId);

        Task<PaginatedList<PatientDetail>> SearchPatientsBySearchQuery(SieveModel sieveModel, string searchQuery);

        Task RecordPatientOrder(PatientOrderRequest patientOrderRequest);

        Task<PatientDetail> UpdatePatientFhirId(Guid patientUuid, Guid patientFhirId);

        Task<IEnumerable<PatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest);
    }
}
