using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService _vehiclesService;

        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehiclesService vehiclesService,
            ILogger<VehiclesController> logger)
        {
            _vehiclesService = vehiclesService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_VEHICLES)]
        [Route("")]
        public async Task<IActionResult> GetAllVehicles([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var vehicles = await _vehiclesService.GetAllVehicles(sieveModel);
                return Ok(vehicles);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting vehicles:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_VEHICLES)]
        [Route("{vehicleId}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] int vehicleId)
        {
            try
            {
                var vehicle = await _vehiclesService.GetVehicleById(vehicleId);
                return Ok(vehicle);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting vehicle with Id ({vehicleId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_VEHICLES)]
        [Route("location/{id}")]
        public async Task<IActionResult> GetVehiclesByLocationId([FromRoute] int id)
        {
            var sites = await _vehiclesService.GetVehiclesByLocationId(id);
            return Ok(sites);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_VEHICLES)]
        [Route("search")]
        public async Task<IActionResult> SearchVehicles([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var vehicles = await _vehiclesService.SearchVehicles(sieveModel, searchQuery);
                return Ok(vehicles);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching vehicles:{ex.Message}");
                throw;
            }
        }
    }
}