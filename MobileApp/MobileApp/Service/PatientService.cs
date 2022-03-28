using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileApp.Assets;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class PatientService
    {
        private readonly IPatientApi _patientApi;

        public PatientService()
        {
            _patientApi = RestService.For<IPatientApi>(HMSHttpClientFactory.GetPatientHttpClient());
        }

        public async Task<PaginatedList<PatientDetail>> GetPatientDetails(IEnumerable<Guid> patientUuids)
        {
            var patientUuidFilterString = string.Join("|", patientUuids);
            var filterString = $"UniqueId == {patientUuidFilterString}";
            try
            {
                var response = await _patientApi.GetPatientDetailsAsync(filterString);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
