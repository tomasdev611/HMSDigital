using Amazon.Runtime.Internal.Util;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers.Integration
{
    public abstract class NetSuiteIntegrationBaseController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ILogger<NetSuiteIntegrationBaseController> _logger;

        public NetSuiteIntegrationBaseController(IUserService userService,
            ILogger<NetSuiteIntegrationBaseController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [NonAction]
        protected async Task<int?> SetAuditUser(string userEmail)
        {
            var apiIdentity = new ClaimsIdentity();
            ViewModels.User user = null;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var userResponse = await _userService.SearchUsers(userEmail, new Sieve.Models.SieveModel());
                user = userResponse.Records.FirstOrDefault();
                if (user == null)
                {
                    var userCreateRequest = new UserCreateRequest()
                    {
                        FirstName = "Unknown",
                        LastName = "User",
                        Email = userEmail.ToLower()
                    };
                    user = await _userService.CreateUser(userCreateRequest);
                }
                apiIdentity.AddClaim(new Claim(Claims.AUDIT_USER_ID_CLAIM, user.Id.ToString(), ClaimValueTypes.Integer32));
            }
            User.AddIdentity(apiIdentity);
            return user?.Id;
        }

        [NonAction]
        protected void DisableGlobalFilter()
        {
            HttpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, GlobalFilters.All);
        }

        [NonAction]
        protected void DisableIsDeletedFilter()
        {
            HttpContext.Items.Add(Claims.IGNORE_IS_DELETED_FILTER, true);
        }

        [NonAction]
        public override BadRequestObjectResult BadRequest(ModelStateDictionary ModelState)
        {
            var errorState = new ErrorState()
            {
                Errors = ModelState.Values.SelectMany(e => e.Errors.Select(em => em.ErrorMessage))
            };
            return BadRequest(errorState);
        }
    }
}
