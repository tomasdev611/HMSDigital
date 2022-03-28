using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using MobileApp.Exceptions;
using MobileApp.Interface;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Methods
{
    public class CurrentLocation
    {
        private readonly PermissionHandler _permissionHandler;

        private readonly ILocationService _locationService;

        public CurrentLocation()
        {
            _permissionHandler = new PermissionHandler();
            _locationService = DependencyService.Get<ILocationService>();
        }

        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var hasLocationPermission = await _permissionHandler.RequestPermissionAsync(PermissionConstants.LOCATION_PERMISSION);
                if (!hasLocationPermission)
                {
                    throw new PermissionRequiredException("Please allow location permission or enable it in settings");
                }
                return await GetLocationAsync();
            }
            catch (PermissionRequiredException pre)
            {
                throw pre;
            }
            catch
            {
                throw;
            }
        }

        private async Task<Location> GetLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                return location;
            }
            catch (FeatureNotEnabledException fex)
            {
                throw new PermissionRequiredException("Please enable location to proceed", fex);
            }
            catch
            {
                throw;
            }
        }

        public bool IsLocationEnabled()
        {

            return _locationService.IsLocationEnabled();
        }

        public async Task<bool> RequestPermissionAsync()
        {
            return await _permissionHandler.RequestPermissionAsync(PermissionConstants.LOCATION_PERMISSION);
        }
    }
}
