using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api")]
    public class HospiceLocationsController : CoreBaseController
    {
        private IHospiceLocationService _hospiceLocationService;

        private ILogger<HospiceLocationsController> _logger;

        public HospiceLocationsController(IHospiceLocationService hospiceLocationService,
            ILogger<HospiceLocationsController> logger)
        {
            _hospiceLocationService = hospiceLocationService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_LOCATIONS)]
        [Route("hospice-locations")]
        [Route("hospices/{hospiceId}/hospice-locations")]
        public async Task<IActionResult> GetAllHospiceLocations([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var hospiceLocations = await _hospiceLocationService.GetAllHospiceLocations(hospiceId, sieveModel);
                return Ok(hospiceLocations);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice locations:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_LOCATIONS)]
        [Route("hospices/{hospiceId}/hospice-locations/{locationId}")]
        public async Task<IActionResult> GetHospiceLocationById([FromRoute] int hospiceId, [FromRoute] int locationId)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
                var hospiceLocation = await _hospiceLocationService.GetHospiceLocationById(locationId);
                if (hospiceLocation == null || hospiceLocation.HospiceId != hospiceId)
                {
                    return NotFound();
                }
                return Ok(hospiceLocation);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice location with id({locationId}):{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_LOCATIONS)]
        [Route("hospices/{hospiceId}/hospice-locations/search")]
        public async Task<IActionResult> SearchHospiceLocations([FromRoute] int hospiceId, [FromQuery] string searchQuery)
        {
            try
            {
                var hospiceLocations = await _hospiceLocationService.SearchHospiceLocations(hospiceId, searchQuery);
                return Ok(hospiceLocations);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching hospice locations:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_LOCATIONS)]
        [Route("hospices/{hospiceId}/hospice-locations/{locationId}/product-catalog")]
        public async Task<IActionResult> GetProductsCatalog([FromRoute] int hospiceId, [FromRoute] int locationId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
                var productsCatalog = await _hospiceLocationService.GetProductsCatalog(hospiceId, locationId, sieveModel);
                return Ok(productsCatalog);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting products catalog with hospice location id({locationId}):{ex.Message}");
                throw;
            }
        }
    }
}