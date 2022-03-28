using HMSDigital.Notification.ViewModels;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger _logger;

        private readonly NotificationHubConfig _notificationHubConfig;

        public PushNotificationService(ILogger<SendGridService> logger,
                                IOptions<NotificationHubConfig> notificationHubConfigOptions)
        {
            _logger = logger;
            _notificationHubConfig = notificationHubConfigOptions.Value;
        }

        public async Task<bool> SendPushNotificationAsync(IEnumerable<string> recipients, string title, string bodyContent, string platform)
        {
            if (recipients == null)
            {
                return false;
            }
            Microsoft.Azure.NotificationHubs.Notification notificationRequest = null;
            var client = new NotificationHubClient(_notificationHubConfig.ConnectionString, _notificationHubConfig.HubName);
            string message;
            switch (platform.ToLower())
            {
                case "ios":
                    var apnsRequest = new ApnsRequest()
                    {
                        Aps = new Aps()
                        {
                            Alert = bodyContent,
                            Sound = "default"
                        }
                    };
                    message = JsonConvert.SerializeObject(apnsRequest);
                    notificationRequest = new AppleNotification(message);
                    break;
                case "android":
                    message = "{\"notification\":{\"body\":\"" + bodyContent + "\"}}";
                    notificationRequest = new Microsoft.Azure.NotificationHubs.FcmNotification(message);
                    break;
            }
            foreach (var deviceToken in recipients)
            {
                var response = await client.SendDirectNotificationAsync(notificationRequest, deviceToken);
                _logger.LogInformation($"send push notification state : {response.State.ToString()}");
            }
            return true;
        }
    }
}
