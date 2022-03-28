using HMSDigital.Common.API.Auth;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using HMSDigital.Notification.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Notification.API.Controllers
{
    [Route("api/notification")]
    [Authorize(Policy = PolicyConstants.CAN_SEND_NOTIFICATION)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostNotification([FromBody] NotificationPostRequest notificationPostRequest)
        {
            await _notificationService.PostNotification(notificationPostRequest);
            return Ok();
        }
    }
}
