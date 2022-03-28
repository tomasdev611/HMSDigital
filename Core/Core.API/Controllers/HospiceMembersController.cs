using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationSDK.Interfaces;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api")]
    public class HospiceMembersController : ControllerBase
    {
        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly ILogger<HospiceMembersController> _logger;

        private readonly IUserAccessService _userAccessService;

        public HospiceMembersController(IHospiceMemberService hospiceMemberService,
            IUserAccessService userAccessService,
            ILogger<HospiceMembersController> logger)
        {
            _hospiceMemberService = hospiceMemberService;
            _logger = logger;
            _userAccessService = userAccessService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members")]
        [Route("members")]
        public async Task<IActionResult> GetAllHospicesMembers([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel, [FromQuery] string roleName)
        {
            try
            {
                var hospiceMembers = await _hospiceMemberService.GetAllHospiceMembers(hospiceId, sieveModel, roleName);
                return Ok(hospiceMembers);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice members:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICE_MEMBERS)]
        [Route("hospices/approver-contacts")]
        public async Task<IActionResult> GetApproverContactsList()
        {
            try
            {
                var approverList = await _hospiceMemberService.GetApproverContacts();
                return Ok(approverList);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice members:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members/{memberId}")]
        public async Task<IActionResult> GetHospiceMembersById([FromRoute] int hospiceId, [FromRoute] int memberId)
        {
            try
            {
                var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(hospiceId, memberId);
                return Ok(hospiceMember);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice member with Id ({memberId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members")]
        public async Task<IActionResult> CreateHospiceMember([FromRoute] int hospiceId, [FromBody] HospiceMemberRequest hospiceMemberRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            HospiceMember hospiceMember;
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                hospiceMember = await _hospiceMemberService.CreateHospiceMember(hospiceId, hospiceMemberRequest);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating hospice member:{ex.Message}");
                throw;
            }
            return CreatedAtAction("GetHospiceMembersById", "hospiceMembers", new { hospiceId = hospiceMember.HospiceId, memberId = hospiceMember.Id }, hospiceMember);
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members/{memberId}")]
        public async Task<IActionResult> UpdateHospiceMember([FromRoute] int hospiceId, [FromRoute] int memberId, [FromBody] HospiceMemberRequest memberUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                var hospiceMember = await _hospiceMemberService.UpdateHospiceMember(hospiceId, memberId, memberUpdateRequest);
                return Ok(hospiceMember);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating hospice member:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members/{memberId}/reset-password")]
        public async Task<IActionResult> SetMemberPassword([FromRoute] int hospiceId, [FromRoute] int memberId, [FromBody] UserPasswordRequest userPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                await _hospiceMemberService.SetMemberPassword(hospiceId, memberId, userPasswordRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while reseting password of hospice member:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members/{memberId}/reset-password-link")]
        public async Task<IActionResult> SendMemberPasswordResetLink([FromRoute] int hospiceId, [FromRoute] int memberId, [FromBody] NotificationBase resetPasswordNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                await _hospiceMemberService.SendMemberPasswordResetLink(hospiceId, memberId, resetPasswordNotification);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while reseting password of hospice member:{ex.Message}");
                throw;
            }
        }


        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members.csv")]
        public async Task<IActionResult> CreateHospiceMembersFromCsvFile([FromRoute] int hospiceId, IFormFile members, [FromQuery] bool validateOnly, [FromQuery] bool parseOnly)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                (var hospiceMembers, var parseList, var validatedList) = await _hospiceMemberService.CreateHospiceMembersFromCsvFile(
                                                                                                    hospiceId,
                                                                                                    members,
                                                                                                    validateOnly,
                                                                                                    parseOnly);
                if (parseList != null)
                {
                    return Ok(parseList);
                }
                if (validatedList != null)
                {
                    return Ok(validatedList);
                }
                return Ok(hospiceMembers);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating hospice member:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_HOSPICE_MEMBERS)]
        [Produces("text/csv")]
        [Route("hospices/{hospiceId}/members.csv")]
        public async Task<IActionResult> GetHospiceMembersAsCsvFile([FromRoute] int hospiceId, [FromQuery] SieveModel sieveModel, [FromQuery] string roleName)
        {
            try
            {
                var canAccessHospice = await _userAccessService.CanAccessHospice(hospiceId);
                if (!canAccessHospice)
                {
                    return Forbid();
                }
                var hospiceMembers = await _hospiceMemberService.GetAllHospiceMembers(hospiceId, sieveModel, roleName);
                return Ok(hospiceMembers.Records);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting hospice member as csv file:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_HOSPICE_MEMBERS)]
        [Route("hospices/{hospiceId}/members/{memberId}")]
        public async Task<IActionResult> DeleteHospiceMember([FromRoute] int hospiceId, [FromRoute] int memberId)
        {
            try
            {
                await _hospiceMemberService.DeleteHospiceMember(hospiceId, memberId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while deleting hospice member:{ex.Message}");
                throw;
            }
        }

    }
}