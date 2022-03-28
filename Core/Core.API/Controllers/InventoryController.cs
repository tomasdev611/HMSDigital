using AutoMapper;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/inventory")]
    public class InventoryController : CoreBaseController
    {
        private readonly IInventoryService _inventoryService;

        private readonly DataBridgeConfig _dataBridgeConfig;

        private readonly IMapper _mapper;

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService,
            IOptions<DataBridgeConfig> dataBridgeOptions,
            IMapper mapper,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _dataBridgeConfig = dataBridgeOptions.Value;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> GetInventory([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var inventory = await _inventoryService.GetAllInventory(sieveModel);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Inventory Items: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("{inventoryId}")]
        public async Task<IActionResult> GetInventoryById(int inventoryId)
        {
            try
            {
                var inventory = await _inventoryService.GetInventoryById(inventoryId);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Inventory Item with Id ({inventoryId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patient/{patientUuid}")]
        public async Task<IActionResult> GetPatientInventory([FromRoute] string patientUuid, [FromQuery] SieveModel sieveModel, [FromQuery] bool includePickupDetails, [FromQuery] bool includeDeliveryAddress)
        {
            try
            {
                if (HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _dataBridgeConfig.ClientId))
                {
                    IgnoreGlobalFilter(Data.Enums.GlobalFilters.All);
                }
                var inventory = await _inventoryService.GetPatientInventory(patientUuid, sieveModel, includePickupDetails, includeDeliveryAddress);
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Patient Inventory Items: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("patient/{patientUuid}")]
        public async Task<IActionResult> AddPatientInventory([FromRoute] Guid patientUuid, [FromBody] PatientInventoryRequest patientInventoryRequest)
        {
            try
            {
                if (!HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _dataBridgeConfig.ClientId))
                {
                    return Forbid();
                }
                IgnoreGlobalFilter(Data.Enums.GlobalFilters.All);
                var patientInventory = await _inventoryService.AddPatientInventory(patientUuid, patientInventoryRequest);
                return Ok(patientInventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Adding patient Inventory: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_PATIENTS)]
        [Route("bulk-patient/{patientUuid}")]
        public async Task<IActionResult> AddBulkPatientInventory([FromRoute] Guid patientUuid, [FromBody] IEnumerable<PatientInventoryRequest> patientInventoryRequests)
        {
            try
            {
                if (!HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _dataBridgeConfig.ClientId))
                {
                    return Forbid();
                }
                IgnoreGlobalFilter(Data.Enums.GlobalFilters.All);
                var patientInventoryResponse = new List<PatientInventoryResponse>();
                foreach (var patientInventoryRequest in patientInventoryRequests)
                {
                    try
                    {
                        var result = await _inventoryService.AddPatientInventory(patientUuid, patientInventoryRequest);
                        patientInventoryResponse.Add(new PatientInventoryResponse()
                        {
                            Success = true,
                            PatientInventory = result,
                            Hms2Id = patientInventoryRequest.Hms2Id
                        });
                    }
                    catch (ValidationException vx)
                    {
                        patientInventoryResponse.Add(new PatientInventoryResponse()
                        {
                            Success = false,
                            Error = vx.Message,
                            PatientInventory = _mapper.Map<PatientInventory>(patientInventoryRequest),
                            Hms2Id = patientInventoryRequest.Hms2Id
                        });
                    }
                }
                return Ok(patientInventoryResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Adding bulk patient Inventory: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryRequest inventoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _inventoryService.CreateInventory(inventoryRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Creating Inventory Item: {ex.Message}");
                throw;
            }
        }

        [HttpPatch]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{inventoryId}")]
        public async Task<IActionResult> PatchInventory([FromRoute] int inventoryId, [FromBody] JsonPatchDocument<Inventory> inventoryPatchDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var inventory = await _inventoryService.PatchInventory(inventoryId, inventoryPatchDocument);
                return Ok(inventory);
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
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating Inventory Item with Id ({inventoryId}): {ex.Message}");
                throw;
            }

        }

        [HttpDelete]
        [Authorize(Policy = PolicyConstants.CAN_DELETE_INVENTORY)]
        [Route("{inventoryId}")]
        public async Task<IActionResult> DeleteInventory([FromRoute] int inventoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _inventoryService.DeleteInventory(inventoryId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting inventory Item with Id ({inventoryId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("search")]
        public async Task<IActionResult> SearchInventoryBySearchQuery([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var inventory = await _inventoryService.SearchInventoryBySearchQuery(sieveModel, searchQuery);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching inventory:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("move")]
        public async Task<IActionResult> MoveInventory([FromBody] MoveInventoryRequest moveInventoryRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    BadRequest(ModelState);
                }
                var inventory = await _inventoryService.MoveInventory(moveInventoryRequest);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while moving Inventory: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_PATIENTS)]
        [Route("patient/{patientUuid}/search")]
        public async Task<IActionResult> SearchPatientInventoryBySearchQuery([FromRoute] string patientUuid, [FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var inventory = await _inventoryService.SearchPatientInventoryBySearchQuery(patientUuid, sieveModel, searchQuery);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching patient inventory:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{inventoryId}")]
        public async Task<IActionResult> UpdateNetSuiteInventory([FromRoute] int inventoryId, [FromBody] NSUpdateInventoryRequest updateNetsuiteInventoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var inventory = await _inventoryService.UpdateNetSuiteInventory(inventoryId, updateNetsuiteInventoryRequest);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating Netsuite inventory:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_INVENTORY)]
        [Route("netSuiteInventory")]
        public async Task<IActionResult> AddNetSuiteInventory([FromBody] NSAddInventoryRequest addInventoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var inventory = await _inventoryService.AddNetSuiteInventory(addInventoryRequest);
                return Ok(inventory);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Adding NetSuite Inventory: {ex.Message}");
                throw;
            }
        }
    }
}
