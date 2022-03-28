using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospiceSource.Digital.NetSuite.SDK.Config;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/")]
    public class DispatchController : CoreBaseController
    {
        private readonly IDispatchService _dispatchService;

        private readonly IFulfillmentService _fulfillmentService;

        private readonly ILogger<DispatchController> _logger;

        private readonly NetSuiteConfig _netSuiteConfig;

        public DispatchController(IDispatchService dispatchService,
            IFulfillmentService fulfillmentService,
            IOptions<NetSuiteConfig> netSuiteOptions,
            ILogger<DispatchController> logger)
        {
            _dispatchService = dispatchService;
            _fulfillmentService = fulfillmentService;
            _logger = logger;
            _netSuiteConfig = netSuiteOptions.Value;
        }


        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_DISPATCH_RECORDS)]
        [Route("dispatch")]
        public async Task<IActionResult> UpdateDispatchRecord([FromBody] IEnumerable<DispatchRecordUpdateRequest> dispatchRecordUpdateRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updateResponse = await _dispatchService.UpdateDispatchRecords(dispatchRecordUpdateRequests);
                return Ok(updateResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating dispatch records : {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("dispatch/loadlist")]
        public async Task<IActionResult> GetLoadlist([FromQuery] int siteId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                if (siteId == 0)
                {
                    return BadRequest($"Site Id ({siteId}) is not valid");
                }
                IgnoreGlobalFilter(GlobalFilters.Hospice);
                var loadlist = await _dispatchService.GetLoadList(siteId, sieveModel);
                return Ok(loadlist);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting loadlist for driver : {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("dispatch/pickup")]
        public async Task<IActionResult> PickupDispatchRequest([FromBody] DispatchMovementRequest pickupRequest)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.All);
                await _dispatchService.PickupDispatchRequest(pickupRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred for pickup of Dispatch request with Id({pickupRequest.RequestId}) : {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("dispatch/drop")]
        public async Task<IActionResult> DropDispatchRequest([FromBody] DispatchMovementRequest completeRequest)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.All);
                await _dispatchService.DropDispatchRequest(completeRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred for completion of Dispatch request with Id({completeRequest.RequestId}) : {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_FULFILL_ORDERS)]
        [Route("dispatch/fulfill-order")]
        public async Task<IActionResult> FulfillOrder([FromBody] OrderFulfillmentRequest fulfillmentRequest)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.All);
                await _fulfillmentService.FulfillOrder(fulfillmentRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred for fulfilment of order with Id({fulfillmentRequest.OrderId}) : {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_FULFILL_ORDERS)]
        [Route("dispatch/update-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderStatusUpdateRequest statusUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _fulfillmentService.UpdateOrderStatus(statusUpdateRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred for updating status of orders with Ids({string.Join(",", statusUpdateRequest.OrderIds)}) : {ex.Message}");
                throw;
            }
        }


        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_DISPATCH_INSTRUCTIONS)]
        [Route("dispatch/assign")]
        public async Task<IActionResult> AssignDispatchToDriver([FromBody] DispatchAssignmentRequest dispatchAssignmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var dispatchInstructions = await _dispatchService.CreateDispatchInstruction(dispatchAssignmentRequest);
                return Ok(dispatchInstructions);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while assigning dispatch items to truck:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_ORDERS)]
        [Route("dispatch/un-assign")]
        public async Task<IActionResult> UnassignOrders([FromBody] int orderId)
        {
            try
            {
                await _dispatchService.UnassignOrder(orderId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while un-assigning order to truck:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("dispatch/me")]
        public async Task<IActionResult> GetLoggedInDriverDispatch(SieveModel sieveModel)
        {
            try
            {
                IgnoreGlobalFilter(GlobalFilters.Site);
                var dispatchResponse = await _dispatchService.GetLoggedInDriverDispatch(sieveModel);
                return Ok(dispatchResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting dispatch Instructions for drivers:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("dispatch/order-locations")]
        public async Task<IActionResult> GetCurrentOrderLocation([FromBody] IEnumerable<int> orderIds)
        {
            try
            {
                var dispatchResponse = await _dispatchService.GetCurrentOrderLocation(orderIds);
                return Ok(dispatchResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting current location for orders:{ex.Message}");
                throw;
            }
        }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_DISPATCH_INSTRUCTIONS)]
        [Route("dispatch-instructions/")]
        public async Task<IActionResult> GetAllDispatchInstructions([FromQuery] SieveModel sieveModel)
        {
            try
            {
                IgnoreDispatchInstructionFilter();
                var dispatchInstructions = await _dispatchService.GetAllDispatchInstructions(sieveModel);
                return Ok(dispatchInstructions);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting dispatch Instructions:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_DISPATCH_INSTRUCTIONS)]
        [Route("dispatch-instructions/{dispatchInstructionId}")]
        public async Task<IActionResult> GetDispatchInstructionById([FromRoute] int dispatchInstructionId)
        {
            try
            {
                var dispatchInstruction = await _dispatchService.GetDispatchInstructionById(dispatchInstructionId);

                return Ok(dispatchInstruction);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting dispatch instruction with Id ({dispatchInstructionId}) :{ex.Message}");
                throw;
            }
        }

        private void IgnoreDispatchInstructionFilter()
        {
            if (HttpContext.User.HasScope(ScopeConstants.ORDER_SCOPE)
                    && HttpContext.User.HasClaim(ClaimTypes.NameIdentifier, _netSuiteConfig.ClientId))
            {
                IgnoreGlobalFilter(GlobalFilters.All);
            }
        }
    }
}