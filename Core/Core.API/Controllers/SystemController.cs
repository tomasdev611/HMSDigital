using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
    [Route("/api/system")]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;

        private readonly ILogger<SystemController> _logger;

        public SystemController(ISystemService systemService, ILogger<SystemController> logger)
        {
            _systemService = systemService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("users")]
        public async Task<IActionResult> GetUsersWithIssues([FromQuery] SieveModel sieveModel, [FromQuery] string issue)
        {
            var userIssues = new List<string> {
                "missing-identity",
                "missing-email"
            };
            if (!ValidateIssues(issue, userIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                issue = issue.ToLower();
                switch (issue)
                {
                    case "missing-identity":
                        var usersWithoutIdentity = await _systemService.GetUsersWithoutIdentity(sieveModel);
                        return Ok(usersWithoutIdentity);

                    case "missing-email":
                        var users = await _systemService.GetUsersWithoutEmail(sieveModel);
                        return Ok(users);
                    default:
                        return BadRequest("No issue specified");
                }

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting users with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("users")]
        public async Task<IActionResult> FixUsersWithIssues([FromBody] SystemUpdateRequest systemUpdateUsersRequest)
        {
            if (!string.Equals(systemUpdateUsersRequest.Issue, "missing-identity", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid issue type: ({systemUpdateUsersRequest.Issue})");
            }
            if (!string.Equals(systemUpdateUsersRequest.Action, "fix", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateUsersRequest.Action})");
            }
            try
            {
                await _systemService.FixAllUsersWithoutIdentity();
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing users with issues:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("address")]
        public async Task<IActionResult> GetNonVerifiedAddress(SieveModel sieveModel, [FromQuery] string issue)
        {
            var addressIssues = new List<string> {
                "non-verified-home-address",
                "non-verified-address"
            };
            if (!ValidateIssues(issue, addressIssues, out var actionResult))
            {
                return actionResult;
            }

            try
            {
                switch (issue)
                {
                    case "non-verified-home-address":
                        var homeAddresses = await _systemService.GetNonVerifiedHomeAddresses(sieveModel);
                        return Ok(homeAddresses);
                    case "non-verified-address":
                        var addresses = await _systemService.GetNonVerifiedAddresses(sieveModel);
                        return Ok(addresses);
                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting non verified address:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("address/{addressId}")]
        public async Task<IActionResult> FixNonVerifiedAddress(int addressId, [FromQuery] string issue)
        {
            try
            {
                var addressIssues = new List<string> {
                    "non-verified-home-address",
                    "non-verified-address"
                };
                if (!ValidateIssues(issue, addressIssues, out var actionResult))
                {
                    return actionResult;
                }

                switch (issue)
                {
                    case "non-verified-home-address":
                        var homeAddresses = await _systemService.FixNonVerifiedHomeAddress(addressId);
                        return Ok(homeAddresses);
                    case "non-verified-address":
                        var addresses = await _systemService.FixNonVerifiedAddress(addressId);
                        return Ok(addresses);
                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing non verified address with Id({addressId}):{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("address")]
        public async Task<IActionResult> FixAddressWithIssue([FromQuery] string issue, [FromBody] SystemUpdateRequest systemUpdateUsersRequest)
        {
            if (!string.Equals(systemUpdateUsersRequest.Issue, "not-verified", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid issue type: ({systemUpdateUsersRequest.Issue})");
            }
            if (!string.Equals(systemUpdateUsersRequest.Action, "fix", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateUsersRequest.Action})");
            }
            try
            {
                var addressIssues = new List<string> {
                    "non-verified-home-address",
                    "non-verified-address"
                };
                if (!ValidateIssues(issue, addressIssues, out var actionResult))
                {
                    return actionResult;
                }

                switch (issue)
                {
                    case "non-verified-home-address":
                        var homeAddresses = await _systemService.FixNonVerifiedHomeAddresses();
                        return Ok(homeAddresses);
                    case "non-verified-address":
                        var addresses = await _systemService.FixNonVerifiedAddresses();
                        return Ok(addresses);
                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing Address with issues:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("members")]
        public async Task<IActionResult> GetMemberWithIssues([FromQuery] string issue, [FromQuery] SieveModel sieveModel)
        {
            var memberIssues = new List<string> {
                "missing-net-suite-contact",
            };
            if (!ValidateIssues(issue, memberIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                var members = await _systemService.GetMemberWithoutNetSuiteContact(sieveModel);
                return Ok(members);

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting members with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("members")]
        public async Task<IActionResult> FixMembersWithIssue([FromBody] SystemUpdateRequest systemUpdateUsersRequest)
        {
            if (!string.Equals(systemUpdateUsersRequest.Issue, "missing-net-suite-contact", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid issue type: ({systemUpdateUsersRequest.Issue})");
            }
            if (!string.Equals(systemUpdateUsersRequest.Action, "fix", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateUsersRequest.Action})");
            }
            try
            {
                var updatedMembersCount = await _systemService.FixMemberWithoutNetSuiteContact();
                return Ok(updatedMembersCount);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing members with issues:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("internal-users")]
        public async Task<IActionResult> GetInternalUsersCountWithIssues([FromQuery] string issue, [FromQuery] SieveModel sieveModel)
        {
            var userIssues = new List<string> {
                "missing-net-suite-contact"
            };
            if (!ValidateIssues(issue, userIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                issue = issue.ToLower();
                switch (issue)
                {
                    case "missing-net-suite-contact":
                        var usersCount = await _systemService.GetInternalUsersWithoutNetSuiteContact(sieveModel);
                        return Ok(usersCount);

                    default:
                        return BadRequest("No issue specified");
                }

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting internal users with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("internal-users")]
        public async Task<IActionResult> FixInternalUsersWithIssues([FromBody] SystemUpdateRequest systemUpdateUsersRequest)
        {
            if (!string.Equals(systemUpdateUsersRequest.Issue, "missing-net-suite-contact", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid issue type: ({systemUpdateUsersRequest.Issue})");
            }
            if (!string.Equals(systemUpdateUsersRequest.Action, "fix", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateUsersRequest.Action})");
            }
            try
            {
                var updatedCount = await _systemService.FixInternalUsersWithoutNetSuiteContact();
                return Ok(updatedCount);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing internal users with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("orders")]
        public async Task<IActionResult> FixOrdersWithIssues([FromQuery] bool dispatchOnly, [FromBody] SystemUpdateRequest systemUpdateOrdersRequest, [FromQuery] int batchSize = 10, [FromQuery] bool stopOnFirstError = true)
        {
            var orderIssues = new List<string> {
                "pending-confirmation-at-netsuite",
                "without-site"
            };

            if (!orderIssues.Any(i => string.Equals(i, systemUpdateOrdersRequest.Issue, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest($"Invalid issue type: ({systemUpdateOrdersRequest.Issue})");
            }
            if (!string.Equals(systemUpdateOrdersRequest.Action, "fix", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateOrdersRequest.Action})");
            }
            try
            {
                var issue = systemUpdateOrdersRequest.Issue.ToLower();
                switch (issue)
                {
                    case "pending-confirmation-at-netsuite":
                        var confirmedOrdersCount = await _systemService.FixUnconfirmedFulfillmentOrders(dispatchOnly, batchSize, stopOnFirstError);
                        return Ok(confirmedOrdersCount);

                    case "without-site":
                        var updatedOrderCount = await _systemService.FixOrdersWithoutSite();
                        return Ok(updatedOrderCount);

                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing orders with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("orders/unconfirmed-order/{orderId}")]
        public async Task<IActionResult> FixUnconfirmedFulfillmentOrder(int orderId, [FromQuery] bool dispatchOnly)
        {
            try
            {
                var isConfirmed = await _systemService.FixUnconfirmedFulfillmentOrder(orderId, dispatchOnly);
                return Ok(isConfirmed);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing uncofirmed order:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("core-api-logs")]
        public async Task<IActionResult> GetCoreApiLogs([FromBody] APILogRequest apiLogRequest)
        {
            try
            {
                var logs = await _systemService.GetCoreApiLogs(apiLogRequest);
                return Ok(logs);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting core api logs:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("orders")]
        public async Task<IActionResult> GetOrdersWithIssues([FromQuery] SieveModel sieveModel, [FromQuery] string issue)
        {
            var orderIssues = new List<string> {
                "invalid-status",
                "without-site",
                "pending-confirmation-at-netsuite"
            };
            if (!ValidateIssues(issue, orderIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                issue = issue.ToLower();
                switch (issue)
                {
                    case "invalid-status":
                        var ordersWithIncorrectStatus = await _systemService.GetOrdersWithIncorrectStatus(sieveModel);
                        return Ok(ordersWithIncorrectStatus);

                    case "without-site":
                        var ordersWithoutSite = await _systemService.GetOrdersWithoutSite(sieveModel);
                        return Ok(ordersWithoutSite);

                    case "pending-confirmation-at-netsuite":
                        var ordersPendingConfirmation = await _systemService.GetUnconfirmedFulfillmentOrders(sieveModel);
                        return Ok(ordersPendingConfirmation);

                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting orders with issue:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("orders/{orderId}")]
        public async Task<IActionResult> FixOrderStatus(int orderId, [FromQuery] bool previewChanges = false)
        {
            try
            {
                var updatedOrder = await _systemService.FixOrderStatus(orderId, previewChanges);
                return Ok(updatedOrder);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing order status:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("patient-inventory")]
        public async Task<IActionResult> GetPatientInventoryWithIssues([FromQuery] string orderNumber, [FromQuery] string assetTagNumber, [FromQuery] string issue)
        {
            var patientInventoryIssues = new List<string> {
                "invalid-inventory",
                "invalid-item"
            };
            if (!ValidateIssues(issue, patientInventoryIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                DisableIsDeletedFilter();

                issue = issue.ToLower();
                switch (issue)
                {
                    case "invalid-inventory":
                        var patientInventoryWithInvalidInventory = await _systemService.GetPatientInventoryWithInvalidInventory(orderNumber, assetTagNumber);
                        return Ok(patientInventoryWithInvalidInventory);

                    case "invalid-item":
                        var patientInventoryWithInvalidItem = await _systemService.GetPatientInventoryWithInvalidItem(orderNumber, assetTagNumber);
                        return Ok(patientInventoryWithInvalidItem);

                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient inventory with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("patient-inventory")]
        public async Task<IActionResult> FixPatientInventoryWithIssues([FromBody] FixPatientInventoryWithIssuesRequest fixRequest, [FromQuery] string issue)
        {
            var patientInventoryIssues = new List<string> {
                "invalid-inventory",
                "invalid-item"
            };
            if (!ValidateIssues(issue, patientInventoryIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                switch (issue)
                {
                    case "invalid-inventory":
                        await _systemService.FixPatientInventoryWithInvalidInventory(fixRequest);
                        return Ok();

                    case "invalid-item":
                        await _systemService.FixPatientInventoryWithInvalidItem(fixRequest);
                        return Ok();

                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient inventory: {ex.Message}");
                throw;
            }
        }


        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("patients")]
        public async Task<IActionResult> GetPatientsWithIssues([FromQuery] string issue)
        {
            var orderIssues = new List<string> {
                "consumable-inventory-only",
                "patient-with-invalid-status-only",
                "missing-fhir-patient"
            };
            if (!ValidateIssues(issue, orderIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                issue = issue.ToLower();
                switch (issue)
                {
                    case "consumable-inventory-only":
                        var ordersPendingConfirmationCount = await _systemService.GetPatientsWithOnlyConsumableInventory();
                        return Ok(ordersPendingConfirmationCount);

                    case "patient-with-invalid-status-only":
                        var invalidStatusPatients = await _systemService.GetPatientsWithInvalidStatus();
                        return Ok(invalidStatusPatients);

                    case "missing-fhir-patient":
                        var missingFhirPatients = await _systemService.GetPatientsWithoutFhirRecord();
                        return Ok(missingFhirPatients);

                    default:
                        return BadRequest("No issue specified");
                }

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting patient with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("patients/{patientUUID}")]
        public async Task<IActionResult> FixPatientsWithIssues([FromRoute] Guid patientUUID, [FromQuery] bool previewChanges, [FromBody] SystemUpdateRequest fixRequest)
        {
            var patientIssues = new List<string> {
                "consumable-inventory-only",
                "patient-with-invalid-status-only",
                "patient-with-invalid-status-all"
            };

            if (!patientIssues.Any(i => string.Equals(i, fixRequest.Issue, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest($"Invalid issue type: ({fixRequest.Issue})");
            }
            if (!string.Equals(fixRequest.Action, "fix", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({fixRequest.Action})");
            }
            try
            {
                var issue = fixRequest.Issue.ToLower();
                switch (issue)
                {
                    case "consumable-inventory-only":
                        var patientInventoryToDelete = await _systemService.FixPatientsWithOnlyConsumableInventory(patientUUID, previewChanges);
                        if (previewChanges)
                        {
                            return Ok(patientInventoryToDelete);
                        }
                        return Ok();

                    case "patient-with-invalid-status-only":
                        var patientStatus = await _systemService.FixPatientWithInvalidStatus(patientUUID, previewChanges);
                        return Ok(patientStatus);
                    case "patient-with-invalid-status-all":
                        await _systemService.FixAllPatientWithStatusIssues();
                        return Ok();
                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating patient inventory: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("patients/fhir-patient")]
        public async Task<IActionResult> FixFhirPatientsWithIssues([FromBody] SystemUpdateRequest fixRequest)
        {
            var patientIssues = new List<string> {
                "missing-fhir-patient"
            };

            if (!patientIssues.Any(i => string.Equals(i, fixRequest.Issue, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest($"Invalid issue type: ({fixRequest.Issue})");
            }
            if (!string.Equals(fixRequest.Action, "fix", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({fixRequest.Action})");
            }
            try
            {
                var issue = fixRequest.Issue.ToLower();
                switch (issue)
                {
                    case "missing-fhir-patient":
                        var updatedCount = await _systemService.FixPatientsWithoutFhirRecord();
                        return Ok(updatedCount);

                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing FHIR patients: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("patient-inventory/deleted")]
        public async Task<IActionResult> GetDeletedPatientInventory([FromQuery] SieveModel sieveModel)
        {
            try
            {
                DisableIsDeletedFilter();
                var deletedPatientInventory = await _systemService.GetDeletedPatientInventory(sieveModel);
                return Ok(deletedPatientInventory);

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting deleted patient inventory:{ex.Message}");
                throw;
            }
        }


        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("netsuite-hms-logs")]
        public async Task<IActionResult> GetNetSuiteLogs([FromBody] NetSuiteLogRequest netsuiteLogRequest)
        {
            try
            {
                var netSuiteLogs = await _systemService.GetNetSuiteLogs(netsuiteLogRequest);
                return Ok(netSuiteLogs);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting netsuite logs:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("netsuite-hms-dispatch")]
        public async Task<IActionResult> GetNetSuiteDispatchRecords([FromBody] NetSuiteDispatchRequest netSuiteDispatchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var netSuiteDispatchRecords = await _systemService.GetNetSuiteDispatchRecords(netSuiteDispatchRequest);
                return Ok(netSuiteDispatchRecords);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting netsuite dispatch records:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SYSTEM)]
        [Route("hospices")]
        public async Task<IActionResult> GetHospicesWithIssues([FromQuery] string issue)
        {
            var hospiceIssues = new List<string> {
                "missing-fhir-organization",
            };
            if (!ValidateIssues(issue, hospiceIssues, out var actionResult))
            {
                return actionResult;
            }
            try
            {
                issue = issue.ToLower();
                switch (issue)
                {
                    case "missing-fhir-organization":
                        var hospicesWithoutFhirOrganization = await _systemService.GetHospicesWithoutFhirOrganization();
                        return Ok(hospicesWithoutFhirOrganization);

                    default:
                        return BadRequest("No issue specified");
                }

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospices with issues:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SYSTEM)]
        [Route("hospices")]
        public async Task<IActionResult> FixHospicesWithIssues([FromBody] SystemUpdateRequest systemUpdateUsersRequest)
        {
            var hospiceIssues = new List<string> {
                "missing-fhir-organization",
            };
            if (!ValidateIssues(systemUpdateUsersRequest.Issue, hospiceIssues, out var actionResult))
            {
                return actionResult;
            }
            if (!string.Equals(systemUpdateUsersRequest.Action, "fix", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid action type: ({systemUpdateUsersRequest.Action})");
            }
            try
            {
                systemUpdateUsersRequest.Issue = systemUpdateUsersRequest.Issue.ToLower();
                switch (systemUpdateUsersRequest.Issue)
                {
                    case "missing-fhir-organization":
                        var updatedCount = await _systemService.FixHospicesWithoutFhirOrganization();
                        return Ok(updatedCount);
                    default:
                        return BadRequest("No issue specified");
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fixing hospices with issues:{ex.Message}");
                throw;
            }
        }

        [NonAction]
        private void DisableIsDeletedFilter()
        {
            HttpContext.Items.Add(Claims.IGNORE_IS_DELETED_FILTER, true);
        }

        private bool ValidateIssues(string issue, IEnumerable<string> issues, out IActionResult actionResult)
        {
            if (string.IsNullOrWhiteSpace(issue)) 
            {
                actionResult = BadRequest($"Issue type is required");
                return false;
            }

            var pattern = string.Join("|", issues);

            if (!new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(issue))
            {
                {
                    actionResult = BadRequest($"Invalid Issue type: ({issue})");
                    return false;
                }
            }

            actionResult = null;
            return true;
        }
    }
}