using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/transfer-requests")]
    public class TransferRequestsController : ControllerBase
    {
        private readonly ITransferRequestsService _transferRequestService;

        private readonly ILogger<TransferRequestsController> _logger;

        public TransferRequestsController(ITransferRequestsService transferRequestsService,
            ILogger<TransferRequestsController> logger)
        {
            _transferRequestService = transferRequestsService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_TRANSFER_REQUESTS)]
        [Route("")]
        public async Task<IActionResult> GetTransferRequests([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var transferRequests = await _transferRequestService.GetAllTransferRequests(sieveModel);
                return Ok(transferRequests);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Transfer Reqeusts: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_TRANSFER_REQUESTS)]
        [Route("{transferRequestId}")]
        public async Task<IActionResult> GetTransferRequestById(int transferRequestId)
        {
            try
            {
                var transferRequest = await _transferRequestService.GetTransferRequestById(transferRequestId);
                return Ok(transferRequest);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting Transfer request with Id ({transferRequestId}): {ex.Message}");
                throw;
            }
        }

    }
}
