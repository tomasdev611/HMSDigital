using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Notification.ViewModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public class NotificationProcessor
    {
        private readonly IEmailService _emailService;

        private readonly ISmsService _smsService;

        private readonly IPushNotificationService _pushNotificationService;

        public NotificationProcessor(IEmailService emailService,
                                     ISmsService smsService,
                                     IPushNotificationService pushNotificationService)
        {
            _emailService = emailService;
            _smsService = smsService;
            _pushNotificationService = pushNotificationService;
        }
        public void ProcessNotification([ServiceBusTrigger("%NotificationQueue:QueueName%", Connection = "NotificationQueue:ListenerConnectionString")] NotificationRequest notificationRequest, ILogger logger)
        {
            SendNotificationAsync(notificationRequest, logger).Wait();
        }

        private async Task SendNotificationAsync(NotificationRequest notificationRequest, ILogger logger)
        {
            if (notificationRequest.Channel == Channels.EMAIL)
            {
                await _emailService.SendEmailAsync(notificationRequest.Recipients, notificationRequest.Subject, notificationRequest.Content, notificationRequest.Content, notificationRequest.Attachments);
            }
            if (notificationRequest.Channel == Channels.SMS)
            {
                await _smsService.SendSmsAsync(notificationRequest.Recipients, notificationRequest.Content);
            }
            if (notificationRequest.Channel == Channels.PUSH)
            {
                await _pushNotificationService.SendPushNotificationAsync(notificationRequest.Recipients, notificationRequest.Subject, notificationRequest.Content,notificationRequest.Platform);
            }
        }

    }
}