using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/sites")]
    public class SitesController : CoreBaseController
    {
        private readonly ISitesService _siteService;

        private readonly ILogger<SitesController> _logger;

        public SitesController(ISitesService sitesService,
            ILogger<SitesController> logger)
        {
            _siteService = sitesService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE)]
        [Route("")]
        public async Task<IActionResult> GetSites([FromQuery] SieveModel sieveModel)
        {
            IgnoreSitesFilter();
            var sites = await _siteService.GetAllSites(sieveModel);
            return Ok(sites);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE)]
        [Route("{siteId}")]
        public async Task<IActionResult> GetSitesById(int siteId)
        {
            try
            {
                IgnoreSitesFilter();
                var site = await _siteService.GetSiteById(siteId);
                return Ok(site);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting site with Id ({siteId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE)]
        [Route("search")]
        public async Task<IActionResult> SearchSites([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                IgnoreSitesFilter();
                var sites = await _siteService.SearchSites(sieveModel, searchQuery);
                return Ok(sites);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching sites:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE)]
        [Route("{siteId}/roles")]
        public async Task<IActionResult> GetInternalRoles([FromRoute] int siteId)
        {
            try
            {
                var internalRoles = await _siteService.GetInternalRoles(siteId);
                return Ok(internalRoles);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting internal roles with Id ({siteId}) :{ex.Message}");
                throw;
            }
        }


        private void IgnoreSitesFilter()
        {
            if (HttpContext.User.HasScope(ScopeConstants.WAREHOUSE_READ_SCOPE))
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
            }
        }
    }
}
