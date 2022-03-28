using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.BusinessLayer.Config;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace HMSDigital.Patient.API.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        private readonly IPatientV2Service _patientV2Service;

        private readonly DataBridgeConfig _dataBridgeConfig;

        private readonly CoreConfig _coreConfig;

        private readonly ILogger<PatientsController> _logger;

        private readonly IFeatureManager _featureManager;

        public PatientsController(
            IPatientService patientService,
            IPatientV2Service patientV2Service,
            IOptions<DataBridgeConfig> dataBridgeOptions,
            IOptions<CoreConfig> coreConfigOptions,
            ILogger<PatientsController> logger,
            IFeatureManager featureManager)
        {
            _patientService = patientService;
            _patientV2Service = patientV2Service;
            _dataBridgeConfig = dataBridgeOptions.Value;
            _coreConfig = coreConfigOptions.Value;
            _logger = logger;
            _featureManager = featureManager;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients")]
        public async Task<IActionResult> GetPatients([FromQuery] SieveModel sieveModel)
        {
            try
            {
                PaginatedList<PatientDetail> patients;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patients = await _patientV2Service.GetAllPatients(sieveModel, IgnorePatientFilter());
                }
                else
                {
                    patients = await _patientService.GetAllPatients(sieveModel, IgnorePatientFilter());
                }
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patients:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/{patientId}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int patientId)
        {
            try
            {
                PatientDetail patient;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patient = await _patientV2Service.GetPatientById(patientId);
                }
                else
                {
                    patient = await _patientService.GetPatientById(patientId);
                }
                return Ok(patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient with Id({patientId}):{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/{patientId}/notes")]
        public async Task<IActionResult> GetPatientNotes([FromRoute] int patientId)
        {
            try
            {
                var patientNotes = await _patientService.GetPatientNotes(patientId, IgnorePatientFilter());
                return Ok(patientNotes);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting notes for patient with Id({patientId}):{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_PATIENTS)]
        [Route("patients")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateRequest patientCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PatientDetail patient;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patient = await _patientService.CreatePatientV2(patientCreateRequest);
                }
                else
                {
                    patient = await _patientService.CreatePatient(patientCreateRequest);
                }
                return CreatedAtAction("GetPatientById", "Patients", new { patientId = patient.Id }, patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while create patients:{ex.Message}");
                throw;
            }
        }

        [HttpPatch]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/{patientId}")]
        public async Task<IActionResult> PatchHospicePatient([FromRoute] int patientId, [FromBody] JsonPatchDocument<PatientDetail> patientPatchDocument, [FromQuery] bool doNotVerifyAddress = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PatientDetail patient;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patient = await _patientService.PatchPatientV2(patientId, patientPatchDocument, doNotVerifyAddress);
                }
                else
                {
                    patient = await _patientService.PatchPatient(patientId, patientPatchDocument, doNotVerifyAddress);
                }
                return Ok(patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (JsonPatchException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException nullReferenceException)
            {
                _logger.LogError(nullReferenceException.Message);
                return BadRequest("Not allow to edit this request");
            }

        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/{patientId}/status")]
        public async Task<IActionResult> UpdatePatientStatus([FromRoute] int patientId, [FromBody] PatientStatusRequest patientStatusRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var patient = await _patientService.UpdatePatientStatus(patientId, patientStatusRequest);
                return Ok(patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient status:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/status")]
        public async Task<IActionResult> UpdatePatientStatusByPatientUuid([FromQuery] Guid patientUuid, [FromBody] PatientStatusRequest patientStatusRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                patientStatusRequest.PatientUuid = patientUuid;
                await _patientService.FixPatientStatus(patientStatusRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient status:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/record-fulfillment")]
        public async Task<IActionResult> RecordOrderFulfillment([FromBody] FulfillmentRecordRequest fulfillmentRecordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _patientService.RecordOrderFulfillment(fulfillmentRecordRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while recording order fulfillment:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/search")]
        public async Task<IActionResult> SearchPatients([FromBody] PatientSearchRequest patientSearchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                IEnumerable<PatientDetail> patients;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patients = await _patientV2Service.SearchPatients(patientSearchRequest);
                }
                else
                {
                    patients = await _patientService.SearchPatients(patientSearchRequest);
                }
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while search pacients:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/search")]
        public async Task<IActionResult> SearchPatientsBySearchQuery([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                PaginatedList<PatientDetail> patients;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    patients = await _patientV2Service.SearchPatientsBySearchQuery(sieveModel, searchQuery);
                }
                else
                {
                    patients = await _patientService.SearchPatientsBySearchQuery(sieveModel, searchQuery);
                }
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching patients:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_RECORD_PATIENT_ORDER)]
        [Route("patients/record-order")]
        public async Task<IActionResult> RecordPatientOrder([FromBody] PatientOrderRequest patientOrderRequest)
        {
            try
            {
                await _patientService.RecordPatientOrder(patientOrderRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while recording patient order:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/hospice/{patientUuid}")]
        public async Task<IActionResult> UpdatePatientHospiceByPatientUuid([FromRoute] Guid patientUuid, [FromBody] PatientHospiceRequest patientHospiceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _patientService.UpdatePatientHospiceByPatientUuid(patientUuid, patientHospiceRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient hospice:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/addresses/{addressUUID}")]
        public async Task<IActionResult> GetPatientAddressByUuid([FromRoute] Guid addressUUID)
        {
            try
            {
                var address = await _patientService.GetPatientAddressByUuid(addressUUID);
                return Ok(address);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient address with UUID({addressUUID}):{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patients/addresses")]
        public async Task<IActionResult> GetNonVerifiedAddresses([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var patients = await _patientService.GetNonVerifiedAddresses(sieveModel);
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patients addresses: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/addresses/{addressId}")]
        public async Task<IActionResult> FixAddressWithIssue([FromRoute] int addressId)
        {
            try
            {
                var patients = await _patientService.FixNonVerifiedAddress(addressId);
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing non verified address with Id({addressId}): {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/addresses")]
        public async Task<IActionResult> FixNonVerifiedAddresses()
        {
            try
            {
                var patients = await _patientService.FixNonVerifiedAddresses();
                return Ok(patients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing Addresses with issues: {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/fhirId")]
        public async Task<IActionResult> UpdatePatientFhirId([FromQuery] Guid patientUuid, [FromBody] Guid fhirId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var patient = await _patientV2Service.UpdatePatientFhirId(patientUuid, fhirId);
                return Ok(patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient fhir id: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/fhir-patient")]
        public async Task<IActionResult> FixMissingFhirPatients()
        {
            try
            {
                var fixedPatientsCount = await _patientService.FixMissingFhirPatients();
                return Ok(fixedPatientsCount);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating FHIR patients: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/validate")]
        public async Task<IActionResult> ValidatePatientStatus([FromBody] IEnumerable<PatientStatusValidationRequest> patientStatusRequest, [FromQuery] bool applyFix)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _patientService.ValidatePatientStatus(patientStatusRequest, applyFix));
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while validating patient status: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/hms2-id")]
        public async Task<IActionResult> UpdateHms2PatientId([FromBody] IEnumerable<PatientLinkRequest> patientLinkRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                if (!HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _dataBridgeConfig.ClientId))
                {
                    return Forbid();
                }

                var patientLinkResponse = new List<PatientLinkResponse>();

                foreach (var patientLink in patientLinkRequests)
                {
                    try
                    {
                        var patient = await _patientService.UpdateHms2PatientId(patientLink.PatientUuid, patientLink.Hms2Id);
                        patientLinkResponse.Add(
                            new PatientLinkResponse()
                            {
                                Success = true,
                                Patient = patient,
                                PatientUuid = patientLink.PatientUuid
                            });
                    }
                    catch (ValidationException vx)
                    {
                        patientLinkResponse.Add(
                            new PatientLinkResponse()
                            {
                                Success = false,
                                Error = vx.Message,
                                PatientUuid = patientLink.PatientUuid
                            });
                    }
                }
                return Ok(patientLinkResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating hms2 patient Id: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patients/{patientUuid}/merge")]
        public async Task<IActionResult> MergePatients([FromRoute] Guid patientUuid, [FromBody] MergePatientRequest mergePatientRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                SetUserIdClaim(mergePatientRequest.MergedByUserId);
                var fhirFeatureIsEnabled = await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE);
                await _patientService.MergePatients(patientUuid, mergePatientRequest, fhirFeatureIsEnabled);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while merging patients: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FINANCE)]
        [Route("patients/merge/history")]
        public async Task<IActionResult> GetPatientMergeHistory([FromQuery] SieveModel sieveModel)
        {
            var patientMergeHistory = await _patientService.GetPatientMergeHistory(sieveModel);
            return Ok(patientMergeHistory);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_PATIENTS)]
        [Route("patients/{patientUuid}/move-patient-to-hospice-location")]
        public async Task<IActionResult> MovePatientToHospiceLocation([FromRoute] Guid patientUuid, [FromQuery] int hospiceId, [FromQuery] int hospiceLocationId, [FromQuery] DateTime movementDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newPatientRecord = await _patientService.MovePatientToHospiceLocation(patientUuid, hospiceId, hospiceLocationId, movementDate);
                return Ok(newPatientRecord);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while moving patient from one hospice to another:{ex.Message}");
                throw;
            }
        }

        private bool IgnorePatientFilter()
        {
            return HttpContext.User.HasScope(ScopeConstants.PATIENT_READ_SCOPE)
                    && (HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _dataBridgeConfig.ClientId)
                        || HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _coreConfig.ClientId));
        }

        private void SetUserIdClaim(int? userId)
        {
            if (!userId.HasValue)
            {
                return;
            }
            var apiIdentity = new ClaimsIdentity();
            apiIdentity.AddClaim(new Claim(Claims.USER_ID_CLAIM, userId.Value.ToString(), ClaimValueTypes.Integer32));
            User.AddIdentity(apiIdentity);
        }
    }
}
