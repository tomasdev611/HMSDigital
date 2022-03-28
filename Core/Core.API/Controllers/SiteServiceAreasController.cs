using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/sites")]
    public class SiteServiceAreasController: ControllerBase
    {
        private readonly ISiteServiceAreaService _siteServiceAreaService;

        private readonly ILogger<SiteMembersController> _logger;

        public SiteServiceAreasController(ISiteServiceAreaService siteServiceAreaService,
                                          ILogger<SiteMembersController> logger)
        {
            _siteServiceAreaService = siteServiceAreaService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE)]
        [Route("{siteId}/service-areas")]
        public async Task<IActionResult> GetServiceAreasBySiteId([FromQuery] SieveModel sieveModel, [FromRoute] int siteId)
        {
            try
            {
                var serviceAreas = await _siteServiceAreaService.GetServiceAreasBySiteId(sieveModel, siteId);
                return Ok(serviceAreas);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting service areas for site with Id ({siteId}): {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_SITE_SERVICE_AREA)]
        [Route("{siteId}/service-areas")]
        public async Task<IActionResult> AddServiceAreaToSite([FromRoute] int siteId, [FromBody] IEnumerable<int> siteServiceAreas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var serviceAreasForSite = await _siteServiceAreaService.CreateServiceAreasForSite(siteId, siteServiceAreas);
                return Ok(serviceAreasForSite);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating service areas for site with Id ({siteId}): {ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize(Policy = PolicyConstants.CAN_DELETE_SITE_SERVICE_AREA)]
        [Route("{siteId}/service-areas")]
        public async Task<IActionResult> DeleteServiceAreaForSite([FromRoute] int siteId, [FromBody] IEnumerable<int> siteServiceAreas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _siteServiceAreaService.DeleteServiceAreasForSite(siteId, siteServiceAreas);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting service areas for site with Id ({siteId}): {ex.Message}");
                throw;
            }
        }
    }
}
