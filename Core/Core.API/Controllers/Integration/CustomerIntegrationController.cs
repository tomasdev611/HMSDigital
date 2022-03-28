using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.API.Controllers.Integration;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HMSDigital.Core.BusinessLayer.Validations;
using Microsoft.FeatureManagement;
using HMSDigital.Common.BusinessLayer.Constants;
using System.Collections.Generic;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize(Policy = PolicyConstants.CAN_CREATE_CUSTOMER_INTEGRATION)]
    [Route("/api/integration")]
    public class CustomerIntegrationController : NetSuiteIntegrationBaseController
    {
        private readonly ILogger<CustomerIntegrationController> _logger;

        private readonly IHospiceService _hospiceService;

        private readonly IHospiceV2Service _hospiceV2Service;

        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly IFeatureManager _featureManager;

        public CustomerIntegrationController(
            IHospiceService hospiceService,
            IHospiceV2Service hospiceV2Service,
            IUserService userService,
            IHospiceMemberService hospiceMemberService,
            ILogger<CustomerIntegrationController> logger,
            IFeatureManager featureManager) : base(userService, logger)
        {
            _logger = logger;
            _hospiceService = hospiceService;
            _hospiceV2Service = hospiceV2Service;
            _hospiceMemberService = hospiceMemberService;
            _featureManager = featureManager;
        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("customer")]
        public async Task<IActionResult> UpsertCustomer([FromBody] NSHospiceRequest hospiceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var hospiceValidator = new HospiceValidator();
                var validationResult = hospiceValidator.Validate(hospiceRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors[0].ErrorMessage);
                }
                DisableGlobalFilter();
                await SetAuditUser(hospiceRequest.CreatedByUserEmail);

                HospiceResponse hospice;
                if (await _featureManager.IsEnabledAsync(FeatureFlagConstants.FHIR_PATIENT_FEATURE))
                {
                    hospice = await _hospiceV2Service.UpsertHospiceWithLocation(hospiceRequest);
                }
                else
                {
                    hospice = await _hospiceService.UpsertHospiceWithLocation(hospiceRequest);
                }
                return Ok(hospice);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating hospice with location and facility:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("customer")]
        public async Task<IActionResult> DeleteCustomer([FromBody] HospiceDeleteRequest hospiceDeleteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                await SetAuditUser(hospiceDeleteRequest.DeletedByUserEmail);
                var customer = await _hospiceService.DeleteCustomer(hospiceDeleteRequest);
                return Ok(customer);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting customer with netsuite Id ({hospiceDeleteRequest.NetSuiteCustomerId}): {ex.Message}");
                throw;
            }

        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("approvers")]
        public async Task<IActionResult> UpsertApprovers([FromBody] ApproverRequest approverRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var approverValidation = new ApproverValidator();
                var validationResult = approverValidation.Validate(approverRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors[0].ErrorMessage);
                }
                DisableGlobalFilter();
                await _hospiceMemberService.UpsertApprovers(approverRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating approval contact:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [HttpPut]
        [Authorize]
        [Route("customer/contract-records")]
        public async Task<IActionResult> UpsertContractRecords([FromBody] IEnumerable<NSContractRecordRequest> contractRecordRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                var contractRecordResponseList = new List<NSContractRecordResponse>();
                foreach (var contractRecordRequest in contractRecordRequests)
                {
                    try
                    {
                        var contractRecordValidator = new ContractRecordValidator();
                        var validationResult = contractRecordValidator.Validate(contractRecordRequest);
                        if (!validationResult.IsValid)
                        {
                            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                        }
                       
                        await SetAuditUser(contractRecordRequest.CreatedByUserEmail);
                        var contractRecordResponse = await _hospiceService.UpsertContractRecord(contractRecordRequest);
                        contractRecordResponseList.Add(
                            new NSContractRecordResponse()
                            {
                                Success = true,
                                HmsContractRecordId = contractRecordResponse.Id,
                                NetSuiteContractRecordId = contractRecordResponse.NetSuiteContractRecordId
                            });
                    }
                    catch (ValidationException vx)
                    {
                        contractRecordResponseList.Add(
                            new NSContractRecordResponse()
                            {
                                Success = false,
                                Error = vx.Message,
                                NetSuiteContractRecordId = contractRecordRequest.NetSuiteContractRecordId
                            });
                    }
                }
                return Ok(new NSContractRecordBulkResponse()
                {
                    Response = contractRecordResponseList
                });

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating/updating contract records:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("customer/contract-records/{netSuiteContractRecordId}")]
        public async Task<IActionResult> DeleteContractRecords([FromRoute] int netSuiteContractRecordId, [FromQuery] string deletedByUserEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DisableGlobalFilter();
                await SetAuditUser(deletedByUserEmail);
                var contractRecordDeleteResponse = await _hospiceService.DeleteContractRecord(netSuiteContractRecordId);
                return Ok(contractRecordDeleteResponse);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting contractRecord with netsuite Id ({netSuiteContractRecordId}): {ex.Message}");
                throw;
            }

        }
    }
}