using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MobileApp.Assets;
using MobileApp.iOS.Service;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms.iOS;

namespace MobileApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if ENABLE_TEST_CLOUD
                Xamarin.Calabash.Start();
#endif

            Popup.Init();
            Forms.Init();
            Platform.Init();

            PushNotificationService.Bootstrap(options);
            LoadApplication(new App());
            AppCenter.Start(AppConfiguration.AppCenteriOS,
                   typeof(Analytics), typeof(Crashes));
            DIPS.Xamarin.UI.iOS.Library.Initialize();
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            PushNotificationService.RefershDeviceRegistration(deviceToken);
        }
    }
}