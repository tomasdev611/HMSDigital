using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/hospices")]
    public class HospicesController : CoreBaseController
    {
        private readonly IHospiceService _hospiceService;

        private readonly IHospiceV2Service _hospiceV2Service;

        private readonly ILogger<HospicesController> _logger;

        public HospicesController(IHospiceService hospiceService,
            IHospiceV2Service hospiceV2Service,
            ILogger<HospicesController> logger)
        {
            _hospiceService = hospiceService;
            _hospiceV2Service = hospiceV2Service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("")]
        public async Task<IActionResult> GetAllHospices([FromQuery] SieveModel sieveModel)
        {
            try
            {
                if (HttpContext.User.HasScope(ScopeConstants.CUSTOMER_READ_SCOPE))
                {
                    IgnoreGlobalFilter(Data.Enums.GlobalFilters.Hospice);
                }

                var hospices = await _hospiceService.GetAllHospices(sieveModel);
                return Ok(hospices);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospices:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("search")]
        public async Task<IActionResult> SearchHospice([FromQuery] SieveModel sieveModel, [FromQuery] string searchQuery)
        {
            try
            {
                if (HttpContext.User.HasScope(ScopeConstants.CUSTOMER_READ_SCOPE))
                {
                    IgnoreGlobalFilter(Data.Enums.GlobalFilters.Hospice);
                }

                var hospices = await _hospiceService.SearchHospices(sieveModel, searchQuery);
                return Ok(hospices);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Searching hospices:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("{hospiceId}")]
        public async Task<IActionResult> GetHospiceById([FromRoute] int hospiceId)
        {
            try
            {
                var hospice = await _hospiceService.GetHospiceById(hospiceId);
                return Ok(hospice);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_CREDIT_ON_HOLD)]
        [Route("{hospiceId}/credit-hold")]
        public async Task<IActionResult> ChangeCreditHoldStatus([FromRoute] int hospiceId, [FromBody] CreditHoldRequest creditHoldRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var hospice = await _hospiceService.ChangeCreditHoldStatus(hospiceId, creditHoldRequest);
                return Ok(hospice);
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
                _logger.LogWarning($"Error Occurred while updating credit hold status for hospice id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("{hospiceId}/credit-hold/history")]
        public async Task<IActionResult> GetCreditHoldHistory([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var creditHoldHistory = await _hospiceService.GetCreditHoldHistory(hospiceId, sieveModel);
                return Ok(creditHoldHistory);
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice credit hold history for hospice id ({hospiceId}) : {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("{hospiceId}/roles")]
        public async Task<IActionResult> GetHospiceRoles([FromRoute] int hospiceId)
        {
            try
            {
                var hospice = await _hospiceService.GetHospiceRoles(hospiceId);
                return Ok(hospice);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice roles with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CUSTOMER_CONTRACT)]
        [Route("{hospiceId}/subscriptions")]
        public async Task<IActionResult> GetHospiceSubscriptions([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var subscriptions = await _hospiceService.GetHospiceSubscriptions(hospiceId, sieveModel);
                return Ok(subscriptions);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice subscriptions with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CUSTOMER_CONTRACT)]
        [Route("{hospiceId}/contracts")]
        public async Task<IActionResult> GetHms2Contracts([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var contracts = await _hospiceService.GetHMS2Contracts(hospiceId, sieveModel);
                return Ok(contracts);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice hms2 contracts with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_CUSTOMER_CONTRACT)]
        [Route("{hospiceId}/subscriptions")]
        public async Task<IActionResult> UpsertHospiceSubscriptions([FromRoute] int hospiceId)
        {
            try
            {
                if (HttpContext.User.HasScope(ScopeConstants.CUSTOMER_SCOPE))
                {
                    IgnoreGlobalFilter(Data.Enums.GlobalFilters.Hospice);
                }
                await _hospiceService.UpsertHospiceSubscriptions(hospiceId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while upserting hospice subscriptions with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_CUSTOMER_CONTRACT)]
        [Route("{hospiceId}/hms2-contracts")]
        public async Task<IActionResult> UpsertHms2HospiceContracts([FromRoute] int hospiceId)
        {
            try
            {
                if (HttpContext.User.HasScope(ScopeConstants.CUSTOMER_SCOPE))
                {
                    IgnoreGlobalFilter(Data.Enums.GlobalFilters.Hospice);
                }
                await _hospiceService.UpsertHms2HospiceContracts(hospiceId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while upserting hms2 hospice contracts with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CUSTOMER_CONTRACT)]
        [Route("subscriptions/{subscriptionId}/subscription-items")]
        public async Task<IActionResult> GetHospiceSubscriptionItemsBySubscription([FromRoute] int subscriptionId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var subscriptionItems = await _hospiceService.GetHospiceSubscriptionItemsBySubscription(subscriptionId, sieveModel);
                return Ok(subscriptionItems);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice subscription items with netSuiteSubscriptionId ({subscriptionId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CUSTOMER_CONTRACT)]
        [Route("contracts/{contractId}/contract-items")]
        public async Task<IActionResult> GetHMS2ContractItemsByContract([FromRoute] int contractId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var contractItems = await _hospiceService.GetHospiceHMS2ContractItemsByContract(contractId, sieveModel);
                return Ok(contractItems);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hms2 contract items for contractId ({contractId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_CUSTOMER_CONTRACT)]
        [Route("contract-records")]
        public async Task<IActionResult> GetHospiceContractRecords([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var contractRecords = await _hospiceService.GetHospiceContractRecords(sieveModel);
                return Ok(contractRecords);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting contract records :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("{hospiceId}/input-mappings")]
        public async Task<IActionResult> GetCsvInputMappingConfig([FromRoute] int hospiceId, [FromQuery] string mappedItemType)
        {

            try
            {
                var inputMapping = await _hospiceService.GetInputCsvMapping(hospiceId, mappedItemType);
                return Ok(inputMapping);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice roles with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICES)]
        [Route("{hospiceId}/output-mappings")]
        public async Task<IActionResult> GetCsvOutputMappingConfig([FromRoute] int hospiceId, [FromQuery] string mappedItemType)
        {

            try
            {
                var outputMapping = await _hospiceService.GetOutputCsvMapping(hospiceId, mappedItemType);
                return Ok(outputMapping);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice roles with Id ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICES)]
        [Route("{hospiceId}/input-mappings")]
        public async Task<IActionResult> PutCsvInputMappingConfig([FromRoute] int hospiceId, [FromQuery] string mappedItemType, [FromBody] CsvMapping<InputMappedItem> inputMapping)
        {
            try
            {
                var mapping = await _hospiceService.PutInputCsvMapping(hospiceId, mappedItemType, inputMapping);
                return Ok(mapping);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while put input csv mapping for hospice Id: ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICES)]
        [Route("{hospiceId}/output-mappings")]
        public async Task<IActionResult> PutCsvOutputMappingConfig([FromRoute] int hospiceId, [FromQuery] string mappedItemType, [FromBody] CsvMapping<OutputMappedItem> outputMapping)
        {
            try
            {
                var mapping = await _hospiceService.PutOutputCsvMapping(hospiceId, mappedItemType, outputMapping);
                return Ok(mapping);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while put output csv mapping for hospice Id: ({hospiceId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Route("fhirOrganizationId")]
        public async Task<IActionResult> UpdateHospiceFhirOrganizationId([FromQuery] int hospiceId, [FromBody] Guid fhirOrganizationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                IgnoreGlobalFilter(Data.Enums.GlobalFilters.Hospice);
                var patient = await _hospiceV2Service.UpdateHospiceFhirOrganizationId(hospiceId, fhirOrganizationId);
                return Ok(patient);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating hospice fhir organization id: {ex.Message}");
                throw;
            }
        }
    }
}