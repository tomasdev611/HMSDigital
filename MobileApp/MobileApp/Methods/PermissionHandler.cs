using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using Xamarin.Essentials;

namespace MobileApp.Methods
{
    public class PermissionHandler
    {
        public async Task<bool> GetPermissionStatusAsync(string permissionType)
        {
            PermissionStatus permissionStatus = PermissionStatus.Unknown;
            switch(permissionType)
            {
                case PermissionConstants.CAMERA_PERMISSION:
                    permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    break;
                case PermissionConstants.PHONE_PERMISSION:
                    permissionStatus = await Permissions.CheckStatusAsync<Permissions.Phone>();
                    break;
                case PermissionConstants.LOCATION_PERMISSION:
                    permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                    break;
            }

            switch(permissionStatus)
            {
                case PermissionStatus.Unknown:
                case PermissionStatus.Denied:
                case PermissionStatus.Disabled:
                    return false;
                case PermissionStatus.Granted:
                    return true;
                default:
                    return false;
            }
        }

        public async Task<bool> RequestPermissionAsync(string permissionType)
        {
            var hasPermission = await GetPermissionStatusAsync(permissionType);
            if(!hasPermission)
            {
                PermissionStatus permissionStatus = PermissionStatus.Unknown;
                switch(permissionType)
                {
                    case PermissionConstants.CAMERA_PERMISSION:
                        permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                        break;
                    case PermissionConstants.PHONE_PERMISSION:
                        permissionStatus = await Permissions.RequestAsync<Permissions.Phone>();
                        break;
                    case PermissionConstants.LOCATION_PERMISSION:
                        permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        break;
                }

                switch (permissionStatus)
                {
                    case PermissionStatus.Unknown:
                    case PermissionStatus.Denied:
                    case PermissionStatus.Disabled:
                        hasPermission = false;
                        break;
                    case PermissionStatus.Granted:
                        hasPermission = true;
                        break;
                    default:
                        hasPermission = false;
                        break;
                }
            }
            return hasPermission;
        }
    }
}
