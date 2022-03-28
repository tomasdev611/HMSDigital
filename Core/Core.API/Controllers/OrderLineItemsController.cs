using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/order-line-items")]
    public class OrderLineItemsController : ControllerBase
    {
        private readonly IOrderLineItemsService _orderLineItemService;

        private readonly ILogger<OrderLineItemsController> _logger;

        public OrderLineItemsController(IOrderLineItemsService orderLineItemsService,
            ILogger<OrderLineItemsController> logger)
        {
            _orderLineItemService = orderLineItemsService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_ORDERS)]
        [Route("")]
        public async Task<IActionResult> GetOrderLineItems()
        {
            var orderLineItems = await _orderLineItemService.GetAllOrderLineItems();
            return Ok(orderLineItems);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_ORDERS)]
        [Route("{orderLineItemId}")]
        public async Task<IActionResult> GetOrderLineItemsById(int orderLineItemId)
        {
            try
            {
                var orderLineItem = await _orderLineItemService.GetOrderLineItemById(orderLineItemId);
                return Ok(orderLineItem);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Order Header with Id ({orderLineItemId}): {ex.Message}");
                throw;
            }
        }
    }
}
