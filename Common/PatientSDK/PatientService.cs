using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.Config;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestEase;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospiceSource.Digital.Patient.SDK
{
    public class PatientService : IPatientService
    {
        private readonly PatientConfig _patientConfig;

        private readonly CoreConfig _coreConfig;

        private readonly ILogger<PatientService> _logger;

        private readonly ITokenService _tokenService;

        private readonly IPatientAPI _patientAPI;

        public PatientService(IOptions<PatientConfig> patientOptions,
                              IOptions<CoreConfig> coreOptions,
                              ILogger<PatientService> logger,
                              ITokenService tokenService)
        {
            _patientConfig = patientOptions.Value;
            _coreConfig = coreOptions.Value;
            _logger = logger;
            _tokenService = tokenService;
            _patientAPI = RestClient.For<IPatientAPI>(_patientConfig.ApiUrl);
        }

        public async Task<PatientDetail> CreatePatient(PatientCreateRequest patientCreateRequest)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.CreatePatient(patientCreateRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to create patient: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create patient: {ex.Message}");
                throw;
            }
        }
        public async Task<PatientDetail> GetPatientByUniqueId(string uniqueId)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={uniqueId}";
                var patients = await _patientAPI.GetPatients(filters);
                return patients.Records.FirstOrDefault();
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patient with uniqueId({uniqueId}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patient with uniqueId({uniqueId}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PatientNote>> GetPatientNotes(Guid uniqueId)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={uniqueId}";
                var patientResponse = await _patientAPI.GetPatients(filters);
                var patient = patientResponse.Records.FirstOrDefault();
                if (patient == null)
                {
                    throw new ValidationException($"Patient with uniqueId({uniqueId}) not found");
                }

                return await _patientAPI.GetPatientNotes(patient.Id);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patient notes with uniqueId({uniqueId}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patient notes with uniqueId({uniqueId}): {ex.Message}");
                throw;
            }
        }

        public async Task<PatientInventory> AddPatientInventory(Guid uniqueId, PatientInventoryRequest patientInventoryRequest)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={uniqueId}";
                var patientResponse = await _patientAPI.GetPatients(filters);
                var patient = patientResponse.Records.FirstOrDefault();
                if (patient == null)
                {
                    throw new ValidationException($"Patient with uniqueId({uniqueId}) not found");
                }

                var coreClient = RestClient.For<ICoreAPI>(_coreConfig.ApiUrl);
                coreClient.BearerToken = _patientAPI.BearerToken;
                return await coreClient.AddPatientInventory(uniqueId, patientInventoryRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while adding patient inventory with uniqueId({uniqueId}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while adding patient inventory with uniqueId({uniqueId}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PatientInventoryResponse>> AddBulkPatientInventory(Guid uniqueId, IEnumerable<PatientInventoryRequest> patientInventoryRequests)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={uniqueId}";
                var patientResponse = await _patientAPI.GetPatients(filters);
                var patient = patientResponse.Records.FirstOrDefault();
                if (patient == null)
                {
                    throw new ValidationException($"Patient with uniqueId({uniqueId}) not found");
                }

                var coreClient = RestClient.For<ICoreAPI>(_coreConfig.ApiUrl);
                coreClient.BearerToken = _patientAPI.BearerToken;
                return await coreClient.AddBulkPatientInventory(uniqueId, patientInventoryRequests);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while adding bulk patient inventory with uniqueId({uniqueId}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while adding bulk patient inventory with uniqueId({uniqueId}): {ex.Message}");
                throw;
            }
        }

        public async Task<PatientDetail> GetPatientByUniqueId(Guid uniqueId)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={uniqueId}";
                var patients = await _patientAPI.GetPatients(filters);
                return patients.Records.FirstOrDefault();
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patient with uniqueId({uniqueId}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patient with uniqueId({uniqueId}): {ex.Message}");
                throw;
            }
        }

        public async Task<PatientDetail> GetPatientByHMS2Id(string hms2Id)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"Hms2Id=={hms2Id}";
                var patients = await _patientAPI.GetPatients(filters);
                return patients.Records.FirstOrDefault();
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patient with Hms2Id({hms2Id}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patient with Hms2Id({hms2Id}): {ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.GetPatients(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patients: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patients: {ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<PatientInventory>> GetPatientInventory(Guid patientUuid, SieveModel sieveModel = null)
        {
            try
            {
                await SetAuthorizationToken();
                var filters = $"UniqueId=={patientUuid}";
                var patientResponse = await _patientAPI.GetPatients(filters);
                var patient = patientResponse.Records.FirstOrDefault();
                if (patient == null)
                {
                    throw new ValidationException($"Patient with patientUuid({patientUuid}) not found");
                }

                var coreClient = RestClient.For<ICoreAPI>(_coreConfig.ApiUrl);
                coreClient.BearerToken = _patientAPI.BearerToken;
                return await coreClient.GetPatientInventory(patientUuid, sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting patient inventory with patientUuid({patientUuid}): {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting patient inventory with patientUuid({patientUuid}): {ex.Message}");
                throw;
            }
        }

        public async Task UpdatePatientStatus(Guid patientUuid, string reason, bool IsDMEEquipmentLeft, bool hasOpenOrders)
        {
            var patientStatusRequest = new PatientStatusRequest()
            {
                StatusChangedDate = DateTime.UtcNow,
                Reason = reason,
                IsDMEEquipmentLeft = IsDMEEquipmentLeft,
                HasOpenOrders = hasOpenOrders
            };
            try
            {
                await SetAuthorizationToken();
                await _patientAPI.UpdatePatientStatusByPatientUuid(patientUuid, patientStatusRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Exception Occurred while updating patient status with uniqueId({patientUuid}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
        }

        public async Task RecordOrderFulfillment(Guid patientUuid, string orderStatus, string pickupReason, bool isDMEEquipmentLeft, bool hasOpenOrders)
        {
            var fulfillmentRecordRequest = new FulfillmentRecordRequest()
            {
                PatientUUID = patientUuid,
                OrderStatus = orderStatus,
                Reason = pickupReason,
                IsDMEEquipmentLeft = isDMEEquipmentLeft,
                HasOpenOrders = hasOpenOrders
            };
            try
            {
                await SetAuthorizationToken();
                await _patientAPI.RecordOrderFulfillment(fulfillmentRecordRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Exception Occurred while recording order fulfillment with uniqueId({patientUuid}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
        }

        public async Task RecordPatientOrder(Guid patientUUID, string orderNumber, bool hasDMEEquipment)
        {
            var patientOrderRequest = new PatientOrderRequest()
            {
                PatientUUID = patientUUID,
                OrderNumber = orderNumber,
                HasDMEEquipment = hasDMEEquipment
            };
            try
            {
                await SetAuthorizationToken();
                await _patientAPI.RecordPatientOrder(patientOrderRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while Sending record patient order request for order number ({orderNumber}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
        }

        public async Task UpdatePatientHospice(Guid patientUuid, int hospiceId, int hospiceLocationId)
        {
            var patientHospiceRequest = new PatientHospiceRequest()
            {
                HospiceId = hospiceId,
                HospiceLocationId = hospiceLocationId,
            };
            try
            {
                await SetAuthorizationToken();
                await _patientAPI.UpdatePatientHospiceByPatientUuid(patientUuid, patientHospiceRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Exception Occurred while updating patient hospice with uniqueId({patientUuid}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient hospice:{ex.Message}");
                throw;
            }
        }

        public async Task<Address> GetPatientAddressByUUID(Guid? addressUUID)
        {
            if (!addressUUID.HasValue)
            {
                return null;
            }
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.GetPatientAddressByUUID(addressUUID.Value);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient address for addressUUID ({addressUUID}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
                return null;
            }
        }

        private async Task SetAuthorizationToken()
        {
            var accessToken = await _tokenService.GetAccessTokenByClientCredentials(_patientConfig.IdentityClient);
            _patientAPI.BearerToken = $"Bearer {accessToken}";
        }

        public async Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.GetNonVerifiedAddresses(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while getting non verified patients addresses: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while getting non verified patients addresses: {ex.Message}");
                throw;
            }
        }

        public async Task<Address> FixAddressWithIssue(int addressId)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.FixAddressWithIssue(addressId);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while fixing non verified patient address: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while fixing non verified patient address: {ex.Message}");
                throw;
            }
        }

        public async Task<long> FixAddressesWithIssue()
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.FixAddressesWithIssue();
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while fixing non verified patients addresses: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while fixing non verified patients addresses: {ex.Message}");
                throw;
            }
        }

        public async Task UpdatePatientFhirId(Guid patientUuid, Guid patientFhirId)
        {
            try
            {
                await SetAuthorizationToken();
                await _patientAPI.UpdatePatientFhirIdByPatientUuid(patientUuid, patientFhirId);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient FHIR id with uniqueId({patientUuid}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while updating patient FHIR id: {ex.Message}");
                throw;
            }
        }

        public async Task<long> FixMissingFhirPatients()
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.FixMissingFhirPatients();
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while fixing missing FHIR patients: {ex.Content}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while fixing missing FHIR patients: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PatientStatusValidationResponse>> ValidatePatientStatus(IEnumerable<PatientStatusValidationRequest> patientStatusValidationRequest, bool applyFix)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.ValidatePatientStatus(patientStatusValidationRequest, applyFix);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while validating patient status:{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while validating patient status: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PatientLinkResponse>> UpdateHms2PatientId(IEnumerable<PatientLinkRequest> patientLinkRequests)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.UpdateHms2PatientId(patientLinkRequests);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while updating hms2 patient Id: {ex.Content}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while updating hms2 patient Id: {ex.Message}");
                throw;
            }
        }

        public async Task<PatientDetail> MergePatient(Guid patientUuid, MergePatientRequest mergePatientRequest)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.MergePatient(patientUuid, mergePatientRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Error Occurred while merging patients: {ex.Content}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while merging patients: {ex.Message}");
                throw;
            }
        }

        public async Task<PatientDetail> MovePatientToHospiceLocation(Guid patientUuid, int hospiceId, int hospiceLocationId, DateTime movementDate)
        {
            try
            {
                await SetAuthorizationToken();
                return await _patientAPI.MovePatientToHospiceLocation(patientUuid, hospiceId, hospiceLocationId, movementDate);
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to move patient from one hospice to another: {ex.Content}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to move patient from one hospice to another: {ex.Message}");
                throw;
            }
        }
    }
}
