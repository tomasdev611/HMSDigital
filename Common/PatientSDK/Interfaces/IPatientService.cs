using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospiceSource.Digital.Patient.SDK.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDetail> CreatePatient(PatientCreateRequest patientCreateRequest);

        Task<PatientDetail> GetPatientByUniqueId(string uniqueId);

        Task<PatientDetail> GetPatientByUniqueId(Guid uniqueId);

        Task<PatientDetail> GetPatientByHMS2Id(string hms2Id);

        Task<IEnumerable<PatientNote>> GetPatientNotes(Guid patientUniqueId);

        Task<PatientInventory> AddPatientInventory(Guid patientUniqueId, PatientInventoryRequest patientInventoryRequest);

        Task<IEnumerable<PatientInventoryResponse>> AddBulkPatientInventory(Guid uniqueId, IEnumerable<PatientInventoryRequest> patientInventoryRequests);

        Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel);

        Task<PaginatedList<PatientInventory>> GetPatientInventory(Guid patientUuid, SieveModel sieveModel = null);

        Task UpdatePatientStatus(Guid patientUuid, string reason, bool IsDMEEquipmentLeft, bool hasOpenOrders);
        Task RecordOrderFulfillment(Guid patientUuid, string orderStatus, string pickupReason, bool isDMEEquipmentLeft,
                                    bool hasOpenOrders);

        Task RecordPatientOrder(Guid patientUUID, string orderNumber, bool hasDMEEquipment);

        Task UpdatePatientHospice(Guid patientUuid, int hospiceId, int hospiceLocationId);

        Task<Address> GetPatientAddressByUUID(Guid? addressUUID);

        Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel);

        Task<Address> FixAddressWithIssue(int addressId);

        Task<long> FixAddressesWithIssue();

        Task UpdatePatientFhirId(Guid patientUuid, Guid patientFhirId);

        Task<long> FixMissingFhirPatients();

        Task<IEnumerable<PatientLinkResponse>> UpdateHms2PatientId(IEnumerable<PatientLinkRequest> patientLinkRequests);

        Task<IEnumerable<PatientStatusValidationResponse>> ValidatePatientStatus(IEnumerable<PatientStatusValidationRequest> patientStatusValidationRequest, bool applyFix);

        Task<PatientDetail> MergePatient(Guid patientUuid, MergePatientRequest mergePatientRequest);

        Task<PatientDetail> MovePatientToHospiceLocation(Guid patientUuid, int hospiceId, int hospiceLocationId, DateTime movementDate);
    }
}
