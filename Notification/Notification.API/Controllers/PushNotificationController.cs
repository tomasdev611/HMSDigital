using HMSDigital.Notification.BusinessLayer.Enums;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using HMSDigital.Notification.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Notification.API.Controllers
{
    [Route("api/push-notification")]
    [Authorize]
    public class PushNotificationController : ControllerBase
    {
        private readonly IPushNotificationService _pushNotificationService;

        private readonly ILogger<PushNotificationController> _logger;

        public PushNotificationController(IPushNotificationService pushNotificationService,
            ILogger<PushNotificationController> logger)
        {
            _pushNotificationService = pushNotificationService;
            _logger = logger;
        }

        [HttpGet]
        [Route("device-register/{deviceId}")]
        public async Task<IActionResult> GetDeviceRegistrationByDeviceId([FromRoute] string deviceId)
        {
            try
            {
                var deviceMetaData = await _pushNotificationService.GetDeviceRegistrationByDeviceId(deviceId);
                return Ok(deviceMetaData);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting register device:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Route("device-register")]
        public async Task<IActionResult> DeviceRegistration([FromBody] DeviceRegister deviceRegisterRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                if (!Enum.TryParse(deviceRegisterRequest.Platform, true, out DevicePlatformTypes type))
                {
                    return BadRequest($"Invalid Device platform: ({deviceRegisterRequest.Platform})");
                }
                await _pushNotificationService.DeviceRegistration(deviceRegisterRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while device register:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Route("device-register/{deviceId}")]
        public async Task<IActionResult> DeleteDeviceRegistration([FromRoute] string deviceId)
        {
            try
            {
                await _pushNotificationService.DeleteDeviceRegistration(deviceId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while delete register device:{ex.Message}");
                throw;
            }
        }
    }
}
