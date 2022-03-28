using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api")]
    public class AuditController : ControllerBase
    {
        private IAuditService _auditService;

        private ILogger<AuditController> _logger;

        public AuditController(IAuditService auditService, ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_AUDIT)]
        [Route("audit")]
        public async Task<IActionResult> GetAuditLogs([FromBody] ViewModels.APILogRequest logRequest)
        {
            try
            {
                var auditLogResponse = await _auditService.GetAuditLogs(logRequest);
                return Ok(auditLogResponse);
            }

            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting audit logs:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_AUDIT)]
        [Route("audit/dispatch-update")]
        public async Task<IActionResult> GetDispatchUpdateAuditLogs([FromQuery] SieveModel sieveModel)
        {
            var dispatchAuditLogs = await _auditService.GetAlldispatchUpdateAuditLogs(sieveModel);
            return Ok(dispatchAuditLogs);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_AUDIT)]
        [Produces("text/csv")]
        [Route("audit.csv")]
        public async Task<IActionResult> GetUsersAuditsAsCsv([FromQuery] SieveModel sieveModel, [FromQuery] string auditType)
        {
            if (!string.Equals(auditType, "User", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid Audit type: ({auditType})");
            }
            var audits = await _auditService.GetAllUsersAuditAsCsvReport(sieveModel);
            return Ok(audits);
        }
    }
}