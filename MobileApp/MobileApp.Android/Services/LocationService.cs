using MobileApp.Droid.Services;
using MobileApp.Interface;
using Xamarin.Forms;
using Android.Locations;
using Android.Content;

[assembly: Dependency(typeof(LocationService))]
namespace MobileApp.Droid.Services
{
    public class LocationService : ILocationService
    {
        public bool IsLocationEnabled()
        {
            var manager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return manager.IsProviderEnabled(LocationManager.GpsProvider);
        }
    }
}
