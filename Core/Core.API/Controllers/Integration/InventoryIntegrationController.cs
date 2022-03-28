using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.API.Controllers.Integration;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize(Policy = PolicyConstants.CAN_CREATE_INVENTORY_INTEGRATION)]
    [Route("/api/integration")]
    public class InventoryIntegrationController : NetSuiteIntegrationBaseController
    {
        private readonly IItemsService _itemsService;

        private readonly IInventoryService _inventoryService;

        private readonly ILogger<InventoryIntegrationController> _logger;

        public InventoryIntegrationController(
            IItemsService itemsService,
            IUserService userService,
            IInventoryService inventoryService,
            ILogger<InventoryIntegrationController> logger) : base(userService, logger)
        {
            _itemsService = itemsService;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("inventory")]
        public async Task<IActionResult> UpsertItem([FromBody] NSItemRequest itemRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var itemValidator = new NSItemValidator();
                var validationResult = itemValidator.Validate(itemRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                DisableGlobalFilter();
                DisableIsDeletedFilter();
                await SetAuditUser(itemRequest.CreatedByUserEmail);
                var itemsResponse = await _itemsService.UpsertItem(itemRequest);
                return Ok(itemsResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating/updating Item:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("inventory")]
        public async Task<IActionResult> DeleteItem([FromBody] ItemDeleteRequest itemDeleteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                await SetAuditUser(itemDeleteRequest.DeletedByUserEmail);
                var itemResponse = await _itemsService.DeleteItemByNetSuiteId(itemDeleteRequest);
                return Ok(itemResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting item with netsuite Id ({itemDeleteRequest.NetSuiteItemId}): {ex.Message}");
                throw;
            }

        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("inventory-item")]
        public async Task<IActionResult> UpsertInventory([FromBody] NSInventoryRequest inventoryRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                DisableIsDeletedFilter();
                await SetAuditUser(inventoryRequest.CreatedByUserEmail);
                var inventoryResponse = await _inventoryService.UpsertInventory(inventoryRequest);
                return Ok(inventoryResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating/updating inventory:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("inventory-item")]
        public async Task<IActionResult> DeleteInventory([FromBody] InventoryDeleteRequest inventoryDeleteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                await SetAuditUser(inventoryDeleteRequest.DeletedByUserEmail);
                var inventoryResponse = await _inventoryService.DeleteInventoryByNetSuiteId(inventoryDeleteRequest);
                return Ok(inventoryResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting inventory with netsuite Id ({inventoryDeleteRequest.NetSuiteInventoryId}): {ex.Message}");
                throw;
            }

        }
    }
}