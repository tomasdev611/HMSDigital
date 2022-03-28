using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> _logger;

        private readonly IUserService _userService;

        public AuthController(IUserService userService,ILogger<AuthController> logger)
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> SignIn(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _userService.SignIn(loginRequest);
                return Ok(result);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while SignIn user:{ex.Message}");
                throw;
            }
        }
    }
}