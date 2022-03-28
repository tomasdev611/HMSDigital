using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/permissions")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsService _permissionsService;

        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IPermissionsService permissionsService,
            ILogger<PermissionsController> logger)
        {
            _permissionsService = permissionsService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_ROLES)]
        [Route("")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _permissionsService.GetAllPermissions();
            return Ok(permissions);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_ROLES)]
        [Route("actions")]
        public async Task<IActionResult> GetPermissionVerbs()
        {
            var permissions = await _permissionsService.GetAllPermissionVerbs();
            return Ok(permissions);
        }

    }
}
