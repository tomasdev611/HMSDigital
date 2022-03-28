using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/finance")]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        private readonly ILogger<FinanceController> _logger;

        public FinanceController(IFinanceService financeService, ILogger<FinanceController> logger)
        {
            _financeService = financeService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_FINANCE)]
        [Route("patients/{patientUUID}/hospice")]
        public async Task<IActionResult> FixPatientHospice([FromRoute] Guid patientUUID, [FromBody] FixPatientHospiceRequest fixPatientHospiceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _financeService.FixPatientHospice(patientUUID, fixPatientHospiceRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing patient hospice: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_FINANCE)]
        [Route("patients/{patientUUID}/merge")]
        public async Task<IActionResult> MergePatient([FromRoute] Guid patientUUID, [FromBody] MergePatientBaseRequest mergePatientRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _financeService.MergePatient(patientUUID, mergePatientRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing patient hospice: {ex.Message}");
                throw;
            }
        }
        
        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_FINANCE)]
        [Route("patients/{patientUUID}/move-patient-to-hospice-location")]
        public async Task<IActionResult> MovePatientToHospiceLocation([FromRoute] Guid patientUUID, [FromBody] MovePatientToHospiceLocationRequest movePatientToHospiceLocationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _financeService.MovePatientToHospiceLocation(patientUUID, movePatientToHospiceLocationRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred while moving patient from hospice: {ex.Message}");
                throw;
            }
        }
    }
}