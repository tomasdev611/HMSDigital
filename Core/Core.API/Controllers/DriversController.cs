using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/drivers")]
    public class DriversController : CoreBaseController
    {
        private readonly IDriversService _driversService;

        private readonly ILogger<DriversController> _logger;

        public DriversController(IDriversService driversService, ILogger<DriversController> logger)
        {
            _driversService = driversService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_DRIVERS)]
        [Route("")]
        public async Task<IActionResult> GetAllDrivers([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var drivers = await _driversService.GetAllDrivers(sieveModel);
                return Ok(drivers);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting drivers:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> GetMyDriver()
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
                var driver = await _driversService.GetMyDriver();
                return Ok(driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in drivers:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize]
        [Route("me/vehicle")]
        public async Task<IActionResult> UpdateMyVehicle([FromQuery] int VehicleId)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
                var driver = await _driversService.UpdateMyVehicle(VehicleId);
                return Ok(driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while update logged in driver vehicle:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_DRIVERS)]
        [Route("{driverId}")]
        public async Task<IActionResult> GetDriverById([FromRoute] int driverId)
        {
            try
            {
                var driver = await _driversService.GetDriverById(driverId);
                return Ok(driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting drivers with Id ({driverId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_DRIVERS)]
        [Route("")]
        public async Task<IActionResult> CreateDriver([FromBody] DriverBase driverCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var driver = await _driversService.CreateDriver(driverCreateRequest);
                return CreatedAtAction("GetDriverById", "drivers", new { driverId = driver.Id }, driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating driver:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_DRIVERS)]
        [Route("{driverId}")]
        public async Task<IActionResult> UpdateDriver([FromRoute] int driverId, [FromBody] DriverBase driverUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var driver = await _driversService.UpdateDriver(driverId, driverUpdateRequest);
                return Ok(driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating driver:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_DRIVERS)]
        [Route("{driverId}")]
        public async Task<IActionResult> DeleteDriver([FromRoute] int driverId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _driversService.DeleteDriver(driverId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting driver:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_DRIVERS)]
        [Route("search")]
        public async Task<IActionResult> SearchDrivers([FromBody] ViewModels.DriversRequest driversRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var drivers = await _driversService.SearchDrivers(driversRequest);
            return Ok(drivers);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_DRIVERS)]
        [Route("search")]
        public async Task<IActionResult> SearchDriversBySearchQuery([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var drivers = await _driversService.SearchDriversBySearchQuery(sieveModel, searchQuery);
                return Ok(drivers);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching drivers:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("me/location")]
        public async Task<IActionResult> UpdateMyLocation([FromBody] GeoLocation location)
        {
            try
            {
                var driver = await _driversService.UpdateMyLocation(location);
                return Ok(driver);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating logged in driver location:{ex.Message}");
                throw;
            }
        }
    }
}
