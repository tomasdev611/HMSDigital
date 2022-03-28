using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
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
    [Route("/api/order-headers")]
    public class OrderHeadersController : CoreBaseController
    {
        private readonly IOrderHeadersService _orderHeaderService;

        private readonly ILogger<OrderHeadersController> _logger;

        public OrderHeadersController(IOrderHeadersService orderHeadersService,
            ILogger<OrderHeadersController> logger)
        {
            _orderHeaderService = orderHeadersService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_ORDERS)]
        [Route("")]
        public async Task<IActionResult> GetAccessibleOrderHeaders([FromQuery] SieveModel sieveModel, [FromQuery] bool includeFulfillmentDetails, [FromQuery] int? locationId)
        {
            try
            {
                var orderHeaders = await _orderHeaderService.GetAllOrderHeaders(sieveModel, includeFulfillmentDetails, locationId);
                return Ok(orderHeaders);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Order Headers: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_ORDERS)]
        [Route("{orderHeaderId}")]
        public async Task<IActionResult> GetOrderHeadersById([FromRoute] int orderHeaderId, [FromQuery] bool includeFulfillmentDetails = false)
        {
            try
            {
                if (includeFulfillmentDetails)
                {
                    IgnoreGlobalFilter(GlobalFilters.Site);
                }
                var orderHeader = await _orderHeaderService.GetOrderHeaderById(orderHeaderId, includeFulfillmentDetails);
                return Ok(orderHeader);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Order Header with Id ({orderHeaderId}): {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_ORDERS)]
        [Route("{orderHeaderId}/fulfillment")]
        public async Task<IActionResult> GetOrderFulfillments(int orderHeaderId, [FromQuery] SieveModel sieveModel)
        {
            var fulfillments = await _orderHeaderService.GetOrderFulfillments(orderHeaderId, sieveModel);
            return Ok(fulfillments);
        }

        [HttpPost]
        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_ORDERS)]
        [Route("")]
        public async Task<IActionResult> UpsertOrderFromWebPortal([FromBody] OrderHeaderRequest orderHeaderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                bool isNetSuiteOrder = false;
                if (orderHeaderRequest.Id != 0)
                {
                    var order = await _orderHeaderService.GetOrderHeaderById(orderHeaderRequest.Id, false);
                    isNetSuiteOrder = order?.NetSuiteOrderId > 0;
                }

                var orderValidator = new WebPortalOrderHeaderValidator(isNetSuiteOrder);
                var validationResult = orderValidator.Validate(orderHeaderRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                var orderResponse = await _orderHeaderService.UpsertOrderFromWebPortal(orderHeaderRequest);
                return Ok(orderResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating order:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_ORDERS)]
        [Route("{orderHeaderId}/order-notes")]
        public async Task<IActionResult> UpdateOrderNotes([FromBody] IEnumerable<UpdateOrderNotesRequest> updateOrderNotesRequest, [FromRoute] int orderHeaderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedOrderHeader = await _orderHeaderService.UpsertOrderNotes(orderHeaderId, updateOrderNotesRequest);
                return Ok(updatedOrderHeader);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating order notes:{ex.Message}");
                throw;
            }
        }
    }
}
