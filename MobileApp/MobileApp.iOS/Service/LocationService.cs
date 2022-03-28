using CoreLocation;
using MobileApp.Interface;
using MobileApp.iOS.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]
namespace MobileApp.iOS.Service
{
    public class LocationService : ILocationService
    {
        public bool IsLocationEnabled()
        {
            return CLLocationManager.LocationServicesEnabled;
        }
    }
}
