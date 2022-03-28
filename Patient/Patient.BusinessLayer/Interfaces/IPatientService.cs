using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Patient.ViewModels.NetSuite;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospiceSource.Digital.Patient.SDK.ViewModels;

namespace HMSDigital.Patient.BusinessLayer.Interfaces
{
    public interface IPatientService
    {
        Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel, bool ignoreFilter = false);

        Task<PatientDetail> GetPatientById(int patientId);

        Task<IEnumerable<PatientNote>> GetPatientNotes(int patientId, bool ignoreFilter);

        Task<PatientLookUp> GetPatientByPatientUuid(string patientUuid);

        Task<PatientDetail> CreatePatient(PatientCreateRequest patientCreateRequest);

        Task<PatientDetail> PatchPatient(int patientId, JsonPatchDocument<PatientDetail> patientPatchDocument, bool doNotVerifyAddress);

        Task<PatientDetail> UpdatePatientStatus(int patientId, PatientStatusRequest patientStatusRequest);

        Task UpdatePatientStatusByPatientUuid(Guid patientUuid, PatientStatusRequest patientStatusRequest);

        Task RecordOrderFulfillment(FulfillmentRecordRequest fulfillmentRecordRequest);

        Task UpdatePatientHospiceByPatientUuid(Guid patientUuid, PatientHospiceRequest patientHospiceRequest);

        Task<IEnumerable<PatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest);

        Task<PaginatedList<PatientDetail>> SearchPatientsBySearchQuery(SieveModel sieveModel, string searchQuery);

        Task RecordPatientOrder(PatientOrderRequest patientOrderRequest);

        Task<Address> GetPatientAddressByUuid(Guid addressUUID);

        Task<PaginatedList<PatientLookUp>> GetAllPatientLookUp(SieveModel sieveModel);

        Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel);

        Task<Address> FixNonVerifiedAddress(int addressId);

        Task<long> FixNonVerifiedAddresses();

        Task<PatientDetail> CreatePatientV2(PatientCreateRequest patientCreateRequest);

        Task<PatientDetail> PatchPatientV2(int patientId, JsonPatchDocument<PatientDetail> patientPatchDocument, bool doNotVerifyAddress);

        Task<long> FixMissingFhirPatients();

        Task FixPatientStatus(PatientStatusRequest patientStatusRequest);

        Task<IEnumerable<PatientStatusValidationResponse>> ValidatePatientStatus(IEnumerable<PatientStatusValidationRequest> patientStatusRequests, bool applyFix);

        Task<PatientDetail> UpdateHms2PatientId(Guid patientUuid, int hms2Id);

        Task<PatientDetail> MergePatients(Guid patientUuid, MergePatientRequest mergePatientRequest, bool updateFhirPatients = false);

        Task<PatientDetail> MovePatientToHospiceLocation(Guid patientUuid, int hospiceId, int hospiceLocationId, DateTime movementDate);

        Task<PaginatedList<PatientMergeHistoryResponse>> GetPatientMergeHistory(SieveModel sieveModel);

    }
}
