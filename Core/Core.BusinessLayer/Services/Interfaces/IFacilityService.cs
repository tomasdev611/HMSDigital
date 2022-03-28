using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IFacilityService
    {
        Task<PaginatedList<Facility>> GetAllFacilities(int hospiceId, SieveModel sieveModel);

        Task<Facility> GetFacilityById(int facilityId);

        Task<IEnumerable<Facility>> SearchFacilities(string searchQuery);

        Task<Facility> CreateFacility(FacilityRequest facilityRequest);

        Task<Facility> PatchFacility(int facilityId, JsonPatchDocument<Facility> facilityPatchDocument);

        Task<IEnumerable<FacilityPatientResponse>> AssignPatientToFacilities(FacilityPatientRequest facilityPatientRequest);

        Task<PaginatedList<FacilityPatientResponse>> GetAllAssignedPatients(int hospiceId, SieveModel sieveModel);

        Task<IEnumerable<FacilityPatientResponse>> GetAssignedPatients(int facilityId);

        Task<IEnumerable<FacilityPatientSearchResponse>> SearchAssignedPatients(int facilityId, SieveModel sieveModel);

        Task<(IEnumerable<Facility>, IEnumerable<FacilityCsvRequest>, ValidatedList<FacilityCsvRequest>)> CreateFacilitiesFromCsvFile(
          int hospiceId,
          IFormFile facilities,
          bool validateOnly,
          bool parseOnly);
    }
}
