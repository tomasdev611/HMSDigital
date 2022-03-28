using FluentValidation;
using HMSDigital.Common.API.Auth;
using HMSDigital.Report.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Threading.Tasks;

namespace HMSDigital.Report.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService,
            ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_METRICS)]
        [Route("ordersMetric")]
        public async Task<IActionResult> GetOrdersMetric([FromQuery] SieveModel sieveModel, [FromQuery]string searchQuery)
        {
            try
            {
                var results = await _reportService.GetOrdersMetric(sieveModel, searchQuery);
                return Ok(results);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting orders metric: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_METRICS)]
        [Route("patientsMetric")]
        public async Task<IActionResult> GetPatientsMetric([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var results = await _reportService.GetPatientsMetric(sieveModel.Filters);
                return Ok(results);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patients metric: {ex.Message}");
                throw;
            }
        }
    }
}
