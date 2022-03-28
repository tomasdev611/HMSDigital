using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Rg.Plugins.Popup;
using MobileApp.Droid.Service;
using Plugin.PushNotification;
using Android.Support.V4.App;
using Android.Content;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MobileApp.Assets;

namespace MobileApp.Droid
{
    [Activity(Label = "HMS Digital", Icon = "@drawable/AppIcon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public readonly string[] Permission =
        {
            Android.Manifest.Permission.ReadCallLog
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            RequestPermissions(Permission, 0);

            new PushNotificationService().Bootstrap(this);
            Popup.Init(this);
            Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            LoadApplication(new App());
            AppCenter.Start(AppConfiguration.AppCenterAndroid,
                typeof(Analytics), typeof(Crashes));
            DIPS.Xamarin.UI.Android.Library.Initialize(this);

            CrossPushNotification.Current.OnNotificationReceived += ShowNotificationInForeground;
        }

        protected override void OnResumeFragments()
        {
            SupportFragmentManager.BeginTransaction().SetTransition((int)FragmentTransit.None);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ShowNotificationInForeground(object source, PushNotificationDataEventArgs eventArgs)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var notification = new NotificationCompat.Builder(this, PushNotificationManager.DefaultNotificationChannelId)
                                .SetContentText(eventArgs.Data["body"].ToString())
                                .SetSmallIcon(Resource.Drawable.AppIcon)
                                .SetDefaults((int)NotificationDefaults.Sound)
                                .Build();

            // Get the notification manager:
            var notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}