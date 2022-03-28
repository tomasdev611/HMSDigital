using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospiceSource.Digital.Patient.SDK.Interfaces
{
    public interface ICoreAPI
    {
        [Header("Authorization", "Bearer")]
        string BearerToken { get; set; }

        [Post("api/inventory/patient/{patientUuid}")]
        Task<PatientInventory> AddPatientInventory([Path] Guid patientUuid, [Body] PatientInventoryRequest patientInventoryRequest);

        [Post("api/inventory/bulk-patient/{patientUuid}")]
        Task<IEnumerable<PatientInventoryResponse>> AddBulkPatientInventory([Path] Guid patientUuid, [Body] IEnumerable<PatientInventoryRequest> patientInventoryRequests);

        [Get("api/inventory/patient/{patientUuid}")]
        Task<PaginatedList<PatientInventory>> GetPatientInventory([Path] Guid patientUuid, [Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);
    }
}
