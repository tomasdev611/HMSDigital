using HMSDigital.Common.API.Auth;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
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
    [Route("/api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        private readonly ILogger<RolesController> _logger;

        public RolesController(IRolesService roleService,
            ILogger<RolesController> logger)
        {
            _rolesService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetAllRoles([FromQuery] SieveModel sieveModel)
        {
            var roles = await _rolesService.GetAllRoles(sieveModel);
            return Ok(roles);
        }

        [HttpGet]
        [Authorize]
        [Route("{roleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute] int roleId)
        {
            var result = await _rolesService.GetRoleById(roleId);
            return Ok(result);
        }
    }
}
