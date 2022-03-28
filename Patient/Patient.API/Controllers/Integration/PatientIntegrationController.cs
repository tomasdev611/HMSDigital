using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Patient.BusinessLayer.Config;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;

namespace HMSDigital.Patient.API.Controllers
{
    [Produces("application/json")]
    [Route("/api/integration/patient")]
    public class PatientIntegrationController : ControllerBase
    {
        private readonly IPatientService _patientService;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly ILogger<PatientIntegrationController> _logger;

        public PatientIntegrationController(
            IPatientService patientService,
            IOptions<NetSuiteConfig> netSuiteOptions,
            ILogger<PatientIntegrationController> logger)
        {
            _logger = logger;
            _netSuiteConfig = netSuiteOptions.Value;
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENT_LOOK_UP)]
        [Route("{patientUuid}")]
        public async Task<IActionResult> GetPatients([FromRoute] string patientUuid)
        {
            try
            {
                if (!HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _netSuiteConfig.ClientId)
                    || !HttpContext.User.HasScope(ScopeConstants.PATIENT_NAME_SCOPE))
                {
                    return Forbid();
                }
                var patientLookUp = await _patientService.GetPatientByPatientUuid(patientUuid);
                return Ok(patientLookUp);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient by uniqueId({patientUuid}):{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENT_LOOK_UP)]
        public async Task<IActionResult> GetPatients([FromQuery] SieveModel sieveModel)
        {
            try
            {
                if (!HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _netSuiteConfig.ClientId)
                    || !HttpContext.User.HasScope(ScopeConstants.PATIENT_NAME_SCOPE))
                {
                    return Forbid();
                }
                var patients = await _patientService.GetAllPatientLookUp(sieveModel);
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
    }
}