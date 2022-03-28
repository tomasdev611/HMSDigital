using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api")]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService;

        private readonly IUserAccessService _userAccessService;

        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(IFacilityService facilityService,
            IUserAccessService userAccessService,
            ILogger<FacilitiesController> logger)
        {
            _facilityService = facilityService;
            _logger = logger;
            _userAccessService = userAccessService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities")]
        [Route("facilities")]
        public async Task<IActionResult> GetAllFacilities([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var facilities = await _facilityService.GetAllFacilities(hospiceId, sieveModel);
                return Ok(facilities);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting facilities:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/{facilityId}")]
        public async Task<IActionResult> GetFacilityById([FromRoute] int hospiceId, [FromRoute] int facilityId)
        {
            try
            {
                var facility = await _facilityService.GetFacilityById(facilityId);
                if (facility == null || facility.HospiceId != hospiceId)
                {
                    return NotFound();
                }
                return Ok(facility);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting facility with Id ({facilityId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities")]
        public async Task<IActionResult> CreateFacility([FromRoute] int hospiceId, [FromBody] FacilityRequest facilityRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                if (facilityRequest.HospiceId != hospiceId)
                {
                    throw new ValidationException($"Invalid hospice Id({facilityRequest.HospiceId})");
                }
                var facility = await _facilityService.CreateFacility(facilityRequest);
                return CreatedAtAction("GetFacilityById", "facilities", new { hospiceId = facility.HospiceId, facilityId = facility.Id }, facility);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating facility:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities.csv")]
        public async Task<IActionResult> CreateFacilitiesFromCsvFile([FromRoute] int hospiceId, IFormFile facilities, [FromQuery] bool validateOnly, [FromQuery] bool parseOnly)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                (var facilitiesResponse, var parseList, var validatedList) = await _facilityService.CreateFacilitiesFromCsvFile(
                                                                                                    hospiceId,
                                                                                                    facilities,
                                                                                                    validateOnly,
                                                                                                    parseOnly);
                if (parseList != null)
                {
                    return Ok(parseList);
                }
                if (validatedList != null)
                {
                    return Ok(validatedList);
                }
                return Ok(facilitiesResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while bulk upload facilities:{ex.Message}");
                throw;
            }
        }


        [HttpPatch]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/{facilityId}")]
        public async Task<IActionResult> PatchFacility([FromRoute] int hospiceId, [FromRoute] int facilityId, [FromBody] JsonPatchDocument<Facility> facilityPatchDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var facility = await _facilityService.GetFacilityById(facilityId);
                if (facility == null)
                {
                    return NotFound();
                }
                if (facility.HospiceId != hospiceId)
                {
                    return Forbid();
                }
                facility = await _facilityService.PatchFacility(facilityId, facilityPatchDocument);
                return Ok(facility);
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

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/patients")]
        public async Task<IActionResult> AssignPatientToFacility([FromRoute] int hospiceId, [FromBody] FacilityPatientRequest facilityPatientRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var facilityPatient = await _facilityService.AssignPatientToFacilities(facilityPatientRequest);
                return Ok(facilityPatient);

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while assign patient to facility:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/patients")]
        public async Task<IActionResult> GetAllAssignedPatients([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var facilityPatients = await _facilityService.GetAllAssignedPatients(hospiceId, sieveModel);
                return Ok(facilityPatients);

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while get assign patient of facility:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/{facilityId}/patients")]
        public async Task<IActionResult> GetAssignedPatients([FromRoute] int hospiceId, [FromRoute] int facilityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var facilityPatients = await _facilityService.GetAssignedPatients(facilityId);
                return Ok(facilityPatients);

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while get assign patient of facility:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/{facilityId}/patients/search")]
        public async Task<IActionResult> SearchAssignedPatients([FromRoute] int hospiceId, [FromRoute] int facilityId, [FromQuery] SieveModel sieveModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                var facility = await _facilityService.GetFacilityById(facilityId);
                if (facility.HospiceId != hospiceId)
                {
                    return BadRequest($"facility Id({facilityId}) not belong to hospice Id({hospiceId})");
                }
                var facilityPatients = await _facilityService.SearchAssignedPatients(facilityId, sieveModel);
                return Ok(facilityPatients);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while assign patient to facility:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_FACILITIES)]
        [Route("hospices/{hospiceId}/facilities/search")]
        public async Task<IActionResult> SearchFacilities([FromRoute] int hospiceId, [FromQuery] string searchQuery)
        {
            try
            {
                var facilities = await _facilityService.SearchFacilities(searchQuery);
                return Ok(facilities);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching facilities:{ex.Message}");
                throw;
            }
        }

    }
}