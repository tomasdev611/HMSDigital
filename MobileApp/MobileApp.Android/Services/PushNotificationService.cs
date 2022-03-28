using MobileApp.Service;
using Plugin.PushNotification;

namespace MobileApp.Droid.Service
{
    public class PushNotificationService
    {
        public void Bootstrap(MainActivity activity)
        {
            PushNotificationManager.Initialize(activity, false);
            RegisterEventHandler();
        }

        private void RefershDeviceRegistration(object source, PushNotificationTokenEventArgs eventArgs)
        {
            var deviceTokenString = eventArgs.Token;

            DeviceTokenService.DeviceToken = deviceTokenString;
            new NotificationService().RefershDeviceRegistration(deviceTokenString);
        }

        private void RegisterEventHandler()
        {
            CrossPushNotification.Current.OnTokenRefresh += RefershDeviceRegistration;
        }
    }
}
