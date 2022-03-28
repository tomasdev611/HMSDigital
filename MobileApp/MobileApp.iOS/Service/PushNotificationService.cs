using Foundation;
using MobileApp.Service;
using Plugin.PushNotification;
using UserNotifications;

namespace MobileApp.iOS.Service
{
    public class PushNotificationService
    {
        public static void Bootstrap(NSDictionary nSDictionary)
        {
            PushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            PushNotificationManager.Initialize(nSDictionary, true);
        }

        public static void RefershDeviceRegistration(NSData deviceToken)
        {
            var deviceTokenString = deviceToken.ToHexString();

            DeviceTokenService.DeviceToken = deviceTokenString;
            new NotificationService().RefershDeviceRegistration(deviceTokenString);
        }
    }
}
