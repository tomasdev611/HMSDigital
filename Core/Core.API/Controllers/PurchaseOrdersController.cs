using HMSDigital.Common.API.Auth;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
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
    [Route("/api/purchase-orders")]
    public class PurchaseOrdersController : CoreBaseController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        private readonly ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, ILogger<PurchaseOrdersController> logger) 
        {
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_INVENTORY)]
        [Route("")]
        public async Task<IActionResult> GetOpenPurchaseOrders([FromQuery] int siteId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                if (siteId <= 0)
                {
                    return BadRequest($"Site Id ({siteId}) is not valid");
                }

                var purchaseOrders = await _purchaseOrderService.GetOpenPurchaseOrders(siteId, sieveModel);
                return Ok(purchaseOrders);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Purchase Orders: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("{netSuitePurchaseOrderId}/receive")]
        public async Task<IActionResult> ReceivePurchaseOrder([FromRoute] int netSuitePurchaseOrderId, [FromBody] ReceivePurchaseOrderRequest receivePurchaseOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var purchaseOrder = await _purchaseOrderService.ReceivePurchaseOrder(netSuitePurchaseOrderId, receivePurchaseOrderRequest);
                return Ok(purchaseOrder);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while completing receipt: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("receipt-images/upload-urls")]
        public async Task<IActionResult> GenerateReceiptImageUploadUrls([FromBody] IEnumerable<ReceiptImageFileRequest> receiptImages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var imagesUrls = await _purchaseOrderService.GetReceiptImageUploadUrls(receiptImages);
                return Ok(imagesUrls);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while generating receipt image upload urls:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_INVENTORY)]
        [Route("receipt-images/download-urls")]
        public async Task<IActionResult> GenerateReceiptImageDownloadUrls([FromBody] IEnumerable<string> storageFilePaths)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var imagesUrls = await _purchaseOrderService.GetReceiptImageDownloadUrls(storageFilePaths);
                return Ok(imagesUrls);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while generating receipt image download urls:{ex.Message}");
                throw;
            }
        }
    }
}
