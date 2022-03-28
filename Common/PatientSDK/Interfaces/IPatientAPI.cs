using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospiceSource.Digital.Patient.SDK.Interfaces
{
    public interface IPatientAPI
    {
        [Header("Authorization", "Bearer")]
        string BearerToken { get; set; }

        [Post("api/patients")]
        Task<PatientDetail> CreatePatient([Body] PatientCreateRequest patientCreateRequest);

        [Get("api/patients")]
        Task<PaginatedList<PatientDetail>> GetPatients([Query] string filters, [Query] string sorts = null, [Query] int? page = null, [Query] int? pageSize = null);

        [Get("api/patients/{patientId}/notes")]
        Task<IEnumerable<PatientNote>> GetPatientNotes([Path] int patientId);

        [Post("api/patients/status")]
        Task UpdatePatientStatusByPatientUuid([Query] Guid patientUuid, [Body] PatientStatusRequest patientStatusRequest);

        [Put("api/patients/hospice/{patientUuid}")]
        Task UpdatePatientHospiceByPatientUuid([Path] Guid patientUuid, [Body] PatientHospiceRequest patientHospiceRequest);

        [Post("api/patients/record-order")]
        Task RecordPatientOrder([Body] PatientOrderRequest patientOrderRequest);

        [Post("api/patients/record-fulfillment")]
        Task RecordOrderFulfillment([Body] FulfillmentRecordRequest fulfillmentRecordRequest);

        [Get("api/patients/addresses/{addressUUID}")]
        Task<Address> GetPatientAddressByUUID([Path] Guid addressUUID);

        [Get("api/patients/addresses")]
        Task<PaginatedList<Address>> GetNonVerifiedAddresses([Query] string filters, [Query] string sorts = null, [Query] int? page = null, [Query] int? pageSize = null);

        [Post("api/patients/addresses/{addressId}")]
        Task<Address> FixAddressWithIssue([Path] int addressId);

        [Post("api/patients/addresses")]
        Task<long> FixAddressesWithIssue();

        [Put("api/patients/fhirId")]
        Task UpdatePatientFhirIdByPatientUuid([Query] Guid patientUuid, [Body] Guid patientFhirId);

        [Post("api/patients/fhir-patient")]
        Task<long> FixMissingFhirPatients();

        [Post("api/patients/hms2-id")]
        Task<IEnumerable<PatientLinkResponse>> UpdateHms2PatientId([Body] IEnumerable<PatientLinkRequest> patientLinkRequests);

        [Post("api/patients/validate")]
        Task<IEnumerable<PatientStatusValidationResponse>> ValidatePatientStatus([Body] IEnumerable<PatientStatusValidationRequest> patientStatusValidationRequest, [Query] bool applyFix);

        [Post("api/patients/{patientUuid}/merge")]
        Task<PatientDetail> MergePatient([Path] Guid patientUuid, [Body] MergePatientRequest mergePatientRequest);

        [Post("api/patients/{patientUuid}/move-patient-to-hospice-location")]
        Task<PatientDetail> MovePatientToHospiceLocation([Path] Guid patientUuid, [Query] int hospiceId, [Query] int hospiceLocationId, [Query] DateTime movementDate);
    }
}
