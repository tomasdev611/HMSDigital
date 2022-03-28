using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsService _itemService;

        private readonly IItemCategoriesService _itemCategoriesService;

        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemsService itemsService,
            IItemCategoriesService itemCategoriesService,
            ILogger<ItemsController> logger)
        {
            _itemService = itemsService;
            _itemCategoriesService = itemCategoriesService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> GetItems([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var items = await _itemService.GetAllItems(sieveModel);
                return Ok(items);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Items: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            try
            {
                var item = await _itemService.GetItemById(itemId);
                return Ok(item);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Item with Id ({itemId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("{itemId}/images")]
        public async Task<IActionResult> GetItemImages(int itemId)
        {
            try
            {
                var itemImages = await _itemService.GetItemImages(itemId);
                return Ok(itemImages);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting images for Item with Id ({itemId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("images")]
        public async Task<IActionResult> GetAllItemImages([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var itemImages = await _itemService.GetAllItemImages(sieveModel);
                return Ok(itemImages);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting images for Items: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{itemId}/transfer")]
        public async Task<IActionResult> TransferItem([FromRoute] int itemId, [FromBody] ItemTransferCreateRequest itemTransferRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _itemService.TransferItem(itemId, itemTransferRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Creating Transfer request for item with Id({itemId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("{itemId}/transfer")]
        public async Task<IActionResult> GetItemTransferRequests([FromRoute] int itemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var transferRequests = await _itemService.GetTransferRequests(itemId);
                return Ok(transferRequests);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Transfer request for item with Id({itemId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("search")]
        public async Task<IActionResult> SearchItems([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var items = await _itemService.SearchItems(sieveModel, searchQuery);
                return Ok(items);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching items:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CATEGORY)]
        [Route("/api/item-categories")]
        public async Task<IActionResult> GetItemCategories([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var itemCategories = await _itemCategoriesService.GetAllItemCategories(sieveModel);
                return Ok(itemCategories);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Item Categories: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CATEGORY)]
        [Route("/api/item-categories/search")]
        public async Task<IActionResult> SearchItemCategories([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                var itemCategories = await _itemCategoriesService.SearchItemCategories(sieveModel, searchQuery);
                return Ok(itemCategories);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching Item Categories: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CATEGORY)]
        [Route("/api/item-categories/{categoryId}")]
        public async Task<IActionResult> GetItemCategoryById(int categoryId)
        {
            try
            {
                var category = await _itemCategoriesService.GetItemCategoryById(categoryId);
                return Ok(category);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Item Category with Id ({categoryId}): {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{itemId}/equipment-config")]
        public async Task<IActionResult> UpdateEquipmentSettingConfig([FromRoute] int itemId, [FromBody] IEnumerable<EquipmentSettingConfig> equipmentSettingsRequest)
        {
            try
            {
                var updatedItem = await _itemService.UpdateEquipmentSettingConfig(itemId, equipmentSettingsRequest);
                return Ok(updatedItem);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Updating Equipment setting config: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("equipment-setting-types")]
        public async Task<IActionResult> GetEquipmentSettingTypes([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var equipmentSettingTypes = await _itemService.GetEquipmentSettingTypes(sieveModel);
                return Ok(equipmentSettingTypes);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Equipment setting types: {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{itemId}/addons-config")]
        public async Task<IActionResult> UpdateAddOnsConfig([FromRoute] int itemId, [FromBody] IEnumerable<AddOnsConfigRequest> addOnsConfigRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedItem = await _itemService.UpdateAddOnsConfig(itemId, addOnsConfigRequests);
                return Ok(updatedItem);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Updating Add ons config: {ex.Message}");
                throw;
            }
        }
    }
}
