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
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/transfer-orders")]
    public class TransferOrdersController : CoreBaseController
    {
        private readonly ITransferOrderService _transferOrderService;

        private readonly ILogger<TransferOrdersController> _logger;

        public TransferOrdersController(ITransferOrderService transferOrderService, ILogger<TransferOrdersController> logger) 
        {
            _transferOrderService = transferOrderService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> GetPendingTransferOrders([FromQuery] int siteId, [FromQuery] bool truckTransferOrders, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                if (siteId <= 0)
                {
                    return BadRequest($"Site Id ({siteId}) is not valid");
                }

                var transferOrders = await _transferOrderService.GetPendingTransferOrders(siteId, truckTransferOrders, sieveModel);
                return Ok(transferOrders);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Pending Transfer Orders: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> CreateTransferOrder([FromBody] TransferOrderCreateRequest transferOrderCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var transferOrder = await _transferOrderService.CreateTransferOrder(transferOrderCreateRequest);
                return Created(string.Empty, transferOrder);
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

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{netSuiteTransferOrderId}/fulfill-receive")]
        public async Task<IActionResult> FulfillReceiveTransferOrder([FromRoute] int netSuiteTransferOrderId, [FromBody] TOrderFulfillReceiveRequest fulfillReceiveRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var transferOrder = await _transferOrderService.FulfillReceiveTransferOrder(netSuiteTransferOrderId, fulfillReceiveRequest);
                return Ok(transferOrder);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while completing the transfer order fulfill/receipt: {ex.Message}");
                throw;
            }
        }
    }
}
