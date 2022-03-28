using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationSDK.Interfaces;
using Sieve.Models;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/sites")]
    public class SiteMembersController : ControllerBase
    {
        private readonly ISiteMemberService _siteMemberService;

        private readonly ILogger<SiteMembersController> _logger;

        private readonly IUserAccessService _userAccessService;

        private readonly INotificationService _notificationService;

        public SiteMembersController(ISiteMemberService siteMemberService,
            IUserAccessService userAccessService,
            INotificationService notificationService,
            ILogger<SiteMembersController> logger)
        {
            _siteMemberService = siteMemberService;
            _logger = logger;
            _userAccessService = userAccessService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE_MEMBERS)]
        [Route("{siteId}/members")]
        public async Task<IActionResult> GetAllSiteMembers([FromRoute] int siteId, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var siteMembers = await _siteMemberService.GetAllSiteMembers(siteId, sieveModel);
                return Ok(siteMembers);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting site members:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("members/me")]
        public async Task<IActionResult> GetMySiteMember()
        {
            try
            {
                var siteMember = await _siteMemberService.GetMySiteMember();
                return Ok(siteMember);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in site member:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> GetMySites()
        {
            try
            {
                var sites = await _siteMemberService.GetMySites();
                return Ok(sites);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in user sites:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_SITE_MEMBERS)]
        [Route("{siteId}/members/{memberId}")]
        public async Task<IActionResult> GetSiteMembersById([FromRoute] int siteId, [FromRoute] int memberId)
        {
            try
            {
                var siteMember = await _siteMemberService.GetSiteMemberById(siteId, memberId);
                if (siteMember == null || siteMember.SiteId != siteId)
                {
                    return NotFound();
                }
                return Ok(siteMember);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting site member with Id ({memberId}) :{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_SITE_MEMBERS)]
        [Route("{siteId}/members")]
        public async Task<IActionResult> CreateSiteMember([FromRoute] int siteId, [FromBody] SiteMemberRequest siteMemberRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SiteMember siteMember;
            try
            {
                siteMember = await _siteMemberService.CreateSiteMember(siteId, siteMemberRequest);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating site member:{ex.Message}");
                throw;
            }
            return CreatedAtAction("GetSiteMembersById", "siteMembers", new { siteId = siteMember.SiteId, memberId = siteMember.Id }, siteMember);
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_SITE_MEMBERS)]
        [Route("{siteId}/members/{memberId}")]
        public async Task<IActionResult> UpdateSiteMember([FromRoute] int siteId, [FromRoute] int memberId, [FromBody] SiteMemberRequest siteMemberUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var siteMember = await _siteMemberService.UpdateSiteMember(siteId, memberId, siteMemberUpdateRequest);
                return Ok(siteMember);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating site member:{ex.Message}");
                throw;
            }
        }
    }
}