using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.API.Controllers.Integration;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize(Policy = PolicyConstants.CAN_CREATE_WAREHOUSE_INTEGRATION)]
    [Route("/api/integration")]
    public class WarehouseIntegrationController : NetSuiteIntegrationBaseController
    {
        private readonly ISitesService _sitesService;

        private readonly ILogger<WarehouseIntegrationController> _logger;

        public WarehouseIntegrationController(
            ISitesService sitesService,
            IUserService userService,
            ILogger<WarehouseIntegrationController> logger) : base(userService, logger)
        {
            _sitesService = sitesService;
            _logger = logger;
        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("warehouse")]
        public async Task<IActionResult> UpsertWarehouse([FromBody] WarehouseRequest warehouseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var warehouseValidator = new WarehouseValidator();
                var validationResult = warehouseValidator.Validate(warehouseRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                DisableGlobalFilter();
                await SetAuditUser(warehouseRequest.CreatedByUserEmail);
                var warehouseResponse = await _sitesService.UpsertWarehouse(warehouseRequest);
                return Ok(warehouseResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating/updating Warehouse:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("warehouse/{netSuiteInternalId}")]
        public async Task<IActionResult> DeleteWarehouse([FromRoute] int netSuiteInternalId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _sitesService.DeleteWarehouse(netSuiteInternalId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting warehouse with netsuite Id ({netSuiteInternalId}): {ex.Message}");
                throw;
            }

        }
    }
}