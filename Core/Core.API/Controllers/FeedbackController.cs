using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSDK.Interfaces;
using NotificationSDK.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("/api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;

        private readonly INotificationService _notificationService;

        private readonly FeedbackConfig _feedbackConfig;

        public FeedbackController(INotificationService notificationService,
                                  IOptions<FeedbackConfig> feedbackConfigOptions,
                                  ILogger<FeedbackController> logger)
        {
            _notificationService = notificationService;
            _feedbackConfig = feedbackConfigOptions.Value;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> CustomerFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            try
            {
                await _notificationService.SendCustomerFeedbackNotification(_feedbackConfig.DefaultToEmail, feedbackRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Sending customer feedback:{ex.Message}");
                throw;
            }
        }
    }
}
