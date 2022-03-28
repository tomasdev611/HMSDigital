using MobileApp.Droid.Service;
using MobileApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceTokenService))]
namespace MobileApp.Droid.Service
{
    class DeviceTokenService : IDeviceTokenService
    {
        public static string DeviceToken { get; set; }
        public string GetDeviceToken()
        {
            return DeviceToken;
        }
    }
}