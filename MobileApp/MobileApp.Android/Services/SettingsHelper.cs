using Android.Content;
using MobileApp.Droid.Services;
using MobileApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsHelper))]
namespace MobileApp.Droid.Services
{
    class SettingsHelper : ISettingsHelper
    {
        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            var uri = Android.Net.Uri.FromParts("package", Android.App.Application.Context.PackageName, null);
            intent.SetData(uri);
            Android.App.Application.Context.StartActivity(intent);
        }

        public void OpenLocationSetting()
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);

            Android.App.Application.Context.StartActivity(intent);
        }
    }
}